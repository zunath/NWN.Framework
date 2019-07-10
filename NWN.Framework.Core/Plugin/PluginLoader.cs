using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

// ReSharper disable once CheckNamespace
namespace SWLOR.Game.Core
{
    /// <summary>
    /// Handles looking for all plugin DLLs in the executing directory. Will load/unload plugins whenever files change.
    /// </summary>
    public class PluginLoader : MarshalByRefObject
    {
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

            MessageHub.Instance.RegisterGlobalHandler(SignalEvent);

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
            //SubscribePluginEvents(pluginRegistration);
            _pluginAppDomains.Add(dllPath, pluginRegistration);

            Console.WriteLine("Registered Plugin: " + plugin.Name);
            MessageHub.Instance.Publish(new OnPluginLoaded(dllPath));
        }

        private void SignalEvent(Type type, object payload)
        {
            foreach (var plugin in _pluginAppDomains)
            {
                plugin.Value.Plugin.SignalEvent(type, Convert.ChangeType(payload, type));
            }
        }

        /// <summary>
        /// Unloads the app domain and plugin from memory.
        /// </summary>
        /// <param name="dllPath">The path to the DLL file.</param>
        private void UnloadPlugin(string dllPath)
        {
            var pluginRegistration = _pluginAppDomains[dllPath];
            pluginRegistration.Plugin.Unregister();
            Console.WriteLine("Unregistered Plugin: " + pluginRegistration.Plugin.Name);
            AppDomain.Unload(pluginRegistration.AppDomain);
            _pluginAppDomains.Remove(dllPath);

            MessageHub.Instance.Publish(new OnPluginUnloaded(dllPath));
        }

    }
}
