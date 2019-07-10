using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using NWN.Framework.Core;
using NWN.Framework.Core.Event.Area;
using NWN.Framework.Core.Event.Creature;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;
using NWN.Scripts;

// ReSharper disable once CheckNamespace
namespace SWLOR.Game.Core
{
    /// <summary>
    /// Handles looking for all plugin DLLs in the executing directory. Will load/unload plugins whenever files change.
    /// </summary>
    public class PluginLoader : MarshalByRefObject
    {
        /// <summary>
        /// Tracks each plugin and which app domain they belong to.
        /// </summary>
        private class PluginRegistration
        {
            public AppDomain AppDomain { get; }
            public IPlugin Plugin { get; }

            public Guid OnModuleAcquireItemID { get; set; }
            public Guid OnModuleActivateItemID { get; set; }
            public Guid OnModuleApplyDamageID { get; set; }
            public Guid OnModuleAttackID { get; set; }
            public Guid OnModuleChatID { get; set; }
            public Guid OnModuleCutsceneAbortID { get; set; }
            public Guid OnModuleDeathID { get; set; }
            public Guid OnModuleDyingID { get; set; }
            public Guid OnModuleEnterID { get; set; }
            public Guid OnModuleEquipItemID { get; set; }
            public Guid OnModuleExamineID { get; set; }
            public Guid OnModuleHeartbeatID { get; set; }
            public Guid OnModuleLeaveID { get; set; }
            public Guid OnModuleLevelUpID { get; set; }
            public Guid OnModuleLoadID { get; set; }
            public Guid OnModuleNWNXChatID { get; set; }
            public Guid OnModuleRespawnID { get; set; }
            public Guid OnModuleRestID { get; set; }
            public Guid OnModuleUnacquireItemID { get; set; }
            public Guid OnModuleUnequipItemID { get; set; }
            public Guid OnModuleUseFeatID { get; set; }
            public Guid OnModuleUserDefinedID { get; set; }

            public Guid OnAreaEnterID { get; set; }
            public Guid OnAreaExitID { get; set; }
            public Guid OnAreaHeartbeatID { get; set; }
            public Guid OnAreaUserDefinedID { get; set; }

            public Guid OnCreatureBlockedID { get; set; }
            public Guid OnCreatureCombatRoundEndID { get; set; }
            public Guid OnCreatureConversationID { get; set; }
            public Guid OnCreatureDamagedID { get; set; }
            public Guid OnCreatureDeathID { get; set; }
            public Guid OnCreatureDisturbedID { get; set; }
            public Guid OnCreatureHeartbeatID { get; set; }
            public Guid OnCreaturePerceptionID { get; set; }
            public Guid OnCreaturePhysicalAttackedID { get; set; }
            public Guid OnCreatureRestedID { get; set; }
            public Guid OnCreatureSpawnID { get; set; }
            public Guid OnCreatureSpellCastAtID { get; set; }
            public Guid OnCreatureUserDefinedID { get; set; }

            public PluginRegistration(AppDomain appDomain, IPlugin plugin)
            {
                AppDomain = appDomain;
                Plugin = plugin;
            }
        }

        /// <summary>
        /// Tracks the active plugin registrations.
        /// </summary>
        private readonly Dictionary<string, PluginRegistration> _pluginAppDomains = new Dictionary<string, PluginRegistration>();

        /// <summary>
        /// Notifies whenever a plugin is added, removed, or changed in the executing directory.
        /// </summary>
        private FileSystemWatcher _watcher;

        /// <summary>
        /// Boots up the plugin loader.
        /// Will also look for any plugins matching the NWN.Framework.Plugin.*.dll file name pattern.
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Plugin loader has started.");

            string fullPath = typeof(PluginLoader).Assembly.Location;
            string directory = Path.GetDirectoryName(fullPath);

            if (directory == null)
            {
                throw new NullReferenceException("Unable to locate assembly directory.");
            }

            // Look for all plugins matching the naming criteria.
            foreach (var plugin in Directory.GetFiles(directory, "NWN.Framework.Plugin.*.dll"))
            {
                try
                {
                    LoadPlugin(plugin);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to load plugin: " + Path.GetFileName(plugin) + ". Error: " + ex);
                }
            }

            StartFileWatcher(directory);
        }

        /// <summary>
        /// Handles watching the executing directory for any plugins that are added, changed, or deleted.
        /// </summary>
        /// <param name="directory"></param>
        private void StartFileWatcher(string directory)
        {
            // Per this post: https://stackoverflow.com/questions/31235034/filesystemwatcher-not-responding-to-file-events-in-folder-shared-by-virtual-mach
            // We have to use a polling configuration or else the file watcher won't pick up file changes.
            Environment.SetEnvironmentVariable("MONO_MANAGED_WATCHER", "1");

            Console.WriteLine("Watching directory: " + directory + " for plugin changes.");
            _watcher = new FileSystemWatcher
            {
                Path = directory,
                Filter = "*.dll",
                NotifyFilter = NotifyFilters.Attributes |
                               NotifyFilters.CreationTime |
                               NotifyFilters.FileName |
                               NotifyFilters.LastAccess |
                               NotifyFilters.LastWrite |
                               NotifyFilters.Size |
                               NotifyFilters.Security
            };

            _watcher.Changed += (sender, args) =>
            {
                UnloadPlugin(args.FullPath);
                LoadPlugin(args.FullPath);
            };
            _watcher.Created += (sender, args) => LoadPlugin(args.FullPath);
            _watcher.Deleted += (sender, args) => UnloadPlugin(args.FullPath);
            _watcher.Renamed += (sender, args) =>
            {
                UnloadPlugin(args.OldFullPath);
                LoadPlugin(args.FullPath);
            };
            _watcher.Error += (sender, args) =>
            {
                Console.WriteLine("ERROR: " + args.GetException().Message);
            };

            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Loads a plugin into memory and runs that plugin's initialization code.
        /// </summary>
        /// <param name="dllPath">Path to the DLL.</param>
        private void LoadPlugin(string dllPath)
        {
            string fileName = Path.GetFileName(dllPath);
            var assemblyName = AssemblyName.GetAssemblyName(dllPath);
            Console.WriteLine("Loading plugin: " + fileName);

            string monoAssemblyPath = Environment.GetEnvironmentVariable("NWNX_MONO_ASSEMBLY");
            AppDomain domain;

            // There won't be a mono assembly if the testing app is being run. Use whatever runs by default on Windows.
            if (string.IsNullOrWhiteSpace(monoAssemblyPath))
            {
                domain = AppDomain.CreateDomain(fileName);
            }
            // But on the game server we'll be running mono. Use the NWNX environment variable for Linux.
            else
            {
                domain = AppDomain.CreateDomain(fileName, null, new AppDomainSetup
                {
                    ApplicationBase = Path.GetDirectoryName(monoAssemblyPath)
                });
            }

            IPlugin plugin = (IPlugin)domain.CreateInstanceAndUnwrap(assemblyName.FullName, assemblyName.Name + ".PluginRegistration");
            plugin.Register();

            // Store the app domain in the dictionary. If the plugin file ever changes, 
            // we'll use this dictionary to reload it.
            var pluginRegistration = new PluginRegistration(domain, plugin);
            SubscribePluginEvents(pluginRegistration);
            _pluginAppDomains.Add(dllPath, pluginRegistration);

            MessageHub.Instance.Publish(new OnPluginLoaded(dllPath));
        }

        /// <summary>
        /// Unloads the app domain and plugin from memory.
        /// </summary>
        /// <param name="dllPath">The path to the DLL file.</param>
        private void UnloadPlugin(string dllPath)
        {
            string fileName = Path.GetFileName(dllPath);
            Console.WriteLine("Unloading plugin: " + fileName);
            var pluginRegistration = _pluginAppDomains[dllPath];
            pluginRegistration.Plugin.Unregister();
            UnsubscribePluginEvents(pluginRegistration);
            AppDomain.Unload(pluginRegistration.AppDomain);
            _pluginAppDomains.Remove(dllPath);

            MessageHub.Instance.Publish(new OnPluginUnloaded(dllPath));
        }

        // Every plugin runs in its own app domain so that we can later unload it, if needed.
        // The recommended way to transfer data between app domains is by inheriting MarshalByRefObject to the objects you want to transfer.
        // As it turns out, Mono is totally incapable of marshalling objects like events and delegates so we're forced to use this awful workaround.
        // You win again, Mono. You win again.

        /// <summary>
        /// Subscribes all events for a given plugin registration.
        /// </summary>
        /// <param name="registration"></param>
        private void SubscribePluginEvents(PluginRegistration registration)
        {
            // Module events
            registration.OnModuleAcquireItemID = MessageHub.Instance.Subscribe<OnModuleAcquireItem>(x => registration.Plugin.OnModuleAcquireItem());
            registration.OnModuleActivateItemID = MessageHub.Instance.Subscribe<OnModuleActivateItem>(x => registration.Plugin.OnModuleActivateItem());
            registration.OnModuleApplyDamageID = MessageHub.Instance.Subscribe<OnModuleApplyDamage>(x => registration.Plugin.OnModuleApplyDamage());
            registration.OnModuleAttackID = MessageHub.Instance.Subscribe<OnModuleAttack>(x => registration.Plugin.OnModuleAttack());
            registration.OnModuleChatID = MessageHub.Instance.Subscribe<OnModuleChat>(x => registration.Plugin.OnModuleChat());
            registration.OnModuleCutsceneAbortID = MessageHub.Instance.Subscribe<OnModuleCutsceneAbort>(x => registration.Plugin.OnModuleCutsceneAbort());
            registration.OnModuleDeathID = MessageHub.Instance.Subscribe<OnModuleDeath>(x => registration.Plugin.OnModuleDeath());
            registration.OnModuleDyingID = MessageHub.Instance.Subscribe<OnModuleDying>(x => registration.Plugin.OnModuleDying());
            registration.OnModuleEnterID = MessageHub.Instance.Subscribe<OnModuleEnter>(x => registration.Plugin.OnModuleEnter());
            registration.OnModuleEquipItemID = MessageHub.Instance.Subscribe<OnModuleEquipItem>(x => registration.Plugin.OnModuleEquipItem());
            registration.OnModuleExamineID = MessageHub.Instance.Subscribe<OnModuleExamine>(x => registration.Plugin.OnModuleExamine());
            registration.OnModuleHeartbeatID = MessageHub.Instance.Subscribe<OnModuleHeartbeat>(x => registration.Plugin.OnModuleHeartbeat());
            registration.OnModuleLeaveID = MessageHub.Instance.Subscribe<OnModuleLeave>(x => registration.Plugin.OnModuleLeave());
            registration.OnModuleLevelUpID = MessageHub.Instance.Subscribe<OnModuleLevelUp>(x => registration.Plugin.OnModuleLevelUp());
            registration.OnModuleLoadID = MessageHub.Instance.Subscribe<OnModuleLoad>(x => registration.Plugin.OnModuleLoad());
            registration.OnModuleNWNXChatID = MessageHub.Instance.Subscribe<OnModuleNWNXChat>(x => registration.Plugin.OnModuleNWNXChat());
            registration.OnModuleRespawnID = MessageHub.Instance.Subscribe<OnModuleRespawn>(x => registration.Plugin.OnModuleRespawn());
            registration.OnModuleRestID = MessageHub.Instance.Subscribe<OnModuleRest>(x => registration.Plugin.OnModuleRest());
            registration.OnModuleUnacquireItemID = MessageHub.Instance.Subscribe<OnModuleUnacquireItem>(x => registration.Plugin.OnModuleUnacquireItem());
            registration.OnModuleUnequipItemID = MessageHub.Instance.Subscribe<OnModuleUnequipItem>(x => registration.Plugin.OnModuleUnequipItem());
            registration.OnModuleUseFeatID = MessageHub.Instance.Subscribe<OnModuleUseFeat>(x => registration.Plugin.OnModuleUseFeat());
            registration.OnModuleUserDefinedID = MessageHub.Instance.Subscribe<OnModuleUserDefined>(x => registration.Plugin.OnModuleUserDefined());

            // Area Events
            registration.OnAreaEnterID = MessageHub.Instance.Subscribe<OnAreaEnter>(x => registration.Plugin.OnAreaEnter());
            registration.OnAreaExitID = MessageHub.Instance.Subscribe<OnAreaExit>(x => registration.Plugin.OnAreaExit());
            registration.OnAreaHeartbeatID = MessageHub.Instance.Subscribe<OnAreaHeartbeat>(x => registration.Plugin.OnAreaHeartbeat());
            registration.OnAreaUserDefinedID = MessageHub.Instance.Subscribe<OnAreaUserDefined>(x => registration.Plugin.OnAreaUserDefined());

            // Creature Events
            registration.OnCreatureBlockedID = MessageHub.Instance.Subscribe<OnCreatureBlocked>(x => registration.Plugin.OnCreatureBlocked());
            registration.OnCreatureCombatRoundEndID = MessageHub.Instance.Subscribe<OnCreatureCombatRoundEnd>(x => registration.Plugin.OnCreatureCombatRoundEnd());
            registration.OnCreatureConversationID = MessageHub.Instance.Subscribe<OnCreatureConversation>(x => registration.Plugin.OnCreatureConversation());
            registration.OnCreatureDamagedID = MessageHub.Instance.Subscribe<OnCreatureDamaged>(x => registration.Plugin.OnCreatureDamaged());
            registration.OnCreatureDeathID = MessageHub.Instance.Subscribe<OnCreatureDeath>(x => registration.Plugin.OnCreatureDeath());
            registration.OnCreatureDisturbedID = MessageHub.Instance.Subscribe<OnCreatureDisturbed>(x => registration.Plugin.OnCreatureDisturbed());
            registration.OnCreatureHeartbeatID = MessageHub.Instance.Subscribe<OnCreatureHeartbeat>(x => registration.Plugin.OnCreatureHeartbeat());
            registration.OnCreaturePerceptionID = MessageHub.Instance.Subscribe<OnCreaturePerception>(x => registration.Plugin.OnCreaturePerception());
            registration.OnCreaturePhysicalAttackedID = MessageHub.Instance.Subscribe<OnCreaturePhysicalAttacked>(x => registration.Plugin.OnCreaturePhysicalAttacked());
            registration.OnCreatureRestedID = MessageHub.Instance.Subscribe<OnCreatureRested>(x => registration.Plugin.OnCreatureRested());
            registration.OnCreatureSpawnID = MessageHub.Instance.Subscribe<OnCreatureSpawn>(x => registration.Plugin.OnCreatureSpawn());
            registration.OnCreatureSpellCastAtID = MessageHub.Instance.Subscribe<OnCreatureSpellCastAt>(x => registration.Plugin.OnCreatureSpellCastAt());
            registration.OnCreatureUserDefinedID = MessageHub.Instance.Subscribe<OnCreatureUserDefined>(x => registration.Plugin.OnCreatureUserDefined());
        }

        /// <summary>
        /// Unsubscribes all events for a given plugin registration.
        /// </summary>
        /// <param name="registration"></param>
        private void UnsubscribePluginEvents(PluginRegistration registration)
        {
            MessageHub.Instance.Unsubscribe(registration.OnModuleAcquireItemID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleActivateItemID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleApplyDamageID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleAttackID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleChatID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleCutsceneAbortID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleDeathID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleDyingID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleEnterID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleEquipItemID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleExamineID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleHeartbeatID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleLeaveID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleLevelUpID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleLoadID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleNWNXChatID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleRespawnID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleRestID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleUnacquireItemID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleUnequipItemID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleUseFeatID);
            MessageHub.Instance.Unsubscribe(registration.OnModuleUserDefinedID);

            MessageHub.Instance.Unsubscribe(registration.OnAreaEnterID);
            MessageHub.Instance.Unsubscribe(registration.OnAreaExitID);
            MessageHub.Instance.Unsubscribe(registration.OnAreaHeartbeatID);
            MessageHub.Instance.Unsubscribe(registration.OnAreaUserDefinedID);
        }

    }
}
