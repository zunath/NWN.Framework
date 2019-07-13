# NWN C# Framework
This is a proof of concept project for a modular C#-based plugin system for Neverwinter Nights.

The idea is that your core server can load or unload plugins as the server is running. Each plugin is put into its own project and is registered automatically with the PluginRegistration class.

The Redis plugin is an example of how this might be used.

This project likely won't be updated again but feel free to fork and expand upon it if you can think of a use for it.


# Dependencies

- Docker
- A Neverwinter Nights module with the "mod_on_load" script set on the module OnLoad event.

An example configuration can be found in the Docker folder.
