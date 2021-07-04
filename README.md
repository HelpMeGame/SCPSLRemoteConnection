# SCPSLRemoteConnection
A plugin for SCP:SL EXILED that allows you to access certain commands from an external socket


## How To Use
By connecting to the configured port on the server, and passing in the correct JSON string (see below), you can gain access to call multiple commands from an external source.

In the EXILED configuration files for the server, you can change the "Viewer Password", and the "Admin Password", as well as change the port, and a couple other options.

Connecting to the Socket with the Viewer password will allow for access to simple, non-intrusive commands such as Player List, and Uptime.

The Admin Password will have access to all Viewer Commands, as well as the Admin commands, such as Ban, Kick, and Broadcast. These commands will need the `allow_commands` option in the configuration to be set to `true`.

JSON Sample Code:
```json
{
   "command":"playerlist",
   "args":[
      
   ],
   "password":"abc"
}
```

A Python Wrapper / Discord Bot demo for this plugin can be downloaded [here](https://github.com/HelpMeGame/RemoteConnectionWrapper)

### Commands
Viewer Commands:
- `playerlist` - Returns a list of players, thier Unique ID, and thier Game ID.
- `uptime` - Returns the number of minutes the server has been running for.
Admin Commands:
- `broadcast` - Broadcasts a message to the server for 5 seconds.
- `ban` - Bans a Player using their Game ID.
- `kick` - Kicks a Player using their Game ID.

### Planned Features
- Event Notices (Restart, Game End, SCP Death)
- More Commands
