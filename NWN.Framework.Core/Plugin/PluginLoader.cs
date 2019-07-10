using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;
using NWN.Scripts;

// ReSharper disable once CheckNamespace
namespace SWLOR.Game.Core
{
    public class PluginLoader: MarshalByRefObject
    {
        private class PluginRegistration
        {
            public AppDomain AppDomain { get; }
            public IPlugin Registration { get; }

            public PluginRegistration(AppDomain appDomain, IPlugin registration)
            {
                AppDomain = appDomain;
                Registration = registration;
            }
        }

        private readonly Dictionary<string, PluginRegistration> _pluginAppDomains = new Dictionary<string, PluginRegistration>();
        private FileSystemWatcher _watcher;

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

            IPlugin plugin = (IPlugin) domain.CreateInstanceAndUnwrap(assemblyName.FullName, assemblyName.Name + ".PluginRegistration");
            plugin.Register();
            plugin.SubscribeEvents(MessageHub.Instance);

            // Store the app domain in the dictionary. If the plugin file ever changes, 
            // we'll use this dictionary to reload it.
            var pluginRegistration = new PluginRegistration(domain, plugin);
            _pluginAppDomains.Add(dllPath, pluginRegistration);

            MessageHub.Instance.Publish(new OnPluginLoaded(dllPath));
        }

        private void UnloadPlugin(string dllPath)
        {
            string fileName = Path.GetFileName(dllPath);
            Console.WriteLine("Unloading plugin: " + fileName);
            var pluginRegistration = _pluginAppDomains[dllPath];
            pluginRegistration.Registration.UnsubscribeEvents(MessageHub.Instance);
            pluginRegistration.Registration.Unregister();
            AppDomain.Unload(pluginRegistration.AppDomain);
            _pluginAppDomains.Remove(dllPath);

            MessageHub.Instance.Publish(new OnPluginUnloaded(dllPath));
        }

    }
}
