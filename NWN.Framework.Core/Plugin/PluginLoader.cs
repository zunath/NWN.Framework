﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;
using NWN.Scripts;

// ReSharper disable once CheckNamespace
namespace SWLOR.Game.Core
{
    internal static class PluginLoader
    {
        private static readonly Dictionary<string, AppDomain> _pluginAppDomains = new Dictionary<string, AppDomain>();
        private static FileSystemWatcher _watcher;

        // ReSharper disable once UnusedMember.Local
        public static void Start()
        {
            Console.WriteLine("Plugin loader has started.");

            string fullPath = typeof(mod_on_load).Assembly.Location;
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
                    Console.WriteLine("Failed to load plugin: " + Path.GetFileName(plugin) + ". Error: " + ex.Message);

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }
                }
            }

            StartFileWatcher(directory);
        }

        private static void StartFileWatcher(string directory)
        {
            Console.WriteLine("Watching directory: " + directory + " for plugin changes.");
            _watcher = new FileSystemWatcher(directory);
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

            _watcher.EnableRaisingEvents = true;
        }

        private static void LoadPlugin(string dllPath)
        {
            string fileName = Path.GetFileName(dllPath);
            Console.WriteLine("Loading plugin: " + fileName);

            // Create an app domain.
            AppDomain domain = AppDomain.CreateDomain(fileName);
            // Run the plugin-specific registration code.
            IPlugin plugin = GetPlugin(dllPath);
            plugin.Register();

            // Store the app domain in the dictionary. If the plugin file ever changes, 
            // we'll use this dictionary to reload it.
            _pluginAppDomains.Add(dllPath, domain);

            MessageHub.Instance.Publish(new OnPluginLoaded(dllPath));
        }

        private static IPlugin GetPlugin(string dllPath)
        {
            Assembly assembly = Assembly.LoadFrom(dllPath);
            // Get all of the types
            Type[] types = assembly.GetTypes();

            // Get the implementation of IPlugin and instantiate it.
            Type pluginType = typeof(IPlugin);
            Type pluginImplementation = types.First(x => pluginType.IsAssignableFrom(x));
            return (IPlugin)Activator.CreateInstance(pluginImplementation);
        }

        private static void UnloadPlugin(string dllPath)
        {
            string fileName = Path.GetFileName(dllPath);
            Console.WriteLine("Unloading plugin: " + fileName);
            var pluginDomain = _pluginAppDomains[dllPath];
            IPlugin plugin = GetPlugin(dllPath);
            plugin.Unregister();
            AppDomain.Unload(pluginDomain);

            MessageHub.Instance.Publish(new OnPluginUnloaded(dllPath));
        }

    }
}