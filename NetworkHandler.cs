using Exiled.API.Features;
using System.Text;
using Utf8Json;
using System.Net.Sockets;
using System.Net;
using System;

namespace RemoteConnection
{
    public class NetworkHandler
    {
        Config config = RemoteConnection.singleton.Config;


        public void StartServer()
        {
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, config.Port);

            try
            {
                Socket listener = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Socket handler = listener.Accept();

                    string data = null;
                    byte[] bytes = null;

                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    byte[] msg = JsonSerializer.Serialize(ProcessCommand(data));
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }

            catch (Exception e)
            {
                Log.Warn(e.ToString());
            }
        }

        public ReturnJson ProcessCommand(string json)
        {
            CommandJson command = JsonSerializer.Deserialize<CommandJson>(json);

            if (command.password == config.ViewerPassword) { command.accessLevel = 1; }
            else if (command.password == config.AdminPassword) { command.accessLevel = 2; }
            else { command.accessLevel = 0; }

            if (command.accessLevel == 0)
            {
                return new ReturnJson("error", "Error: Password Incorrect.", 0);
            }

            string commandType = "error";
            string msg = "Error: An Unknown Error Occured.";
            int success = 0;
            bool gotResponse = false;

            if (command.accessLevel > 0)
            {
                if (command.command.ToLower() == "playerlist") { msg = CommandHandler.GetPlayerList(); commandType = "playerlist"; success = 1; gotResponse = true; }
                else if (command.command.ToLower() == "uptime") { msg = CommandHandler.GetUptime(); commandType = "uptime"; success = 1; gotResponse = true; }
            }
            else { return new ReturnJson("error", "Error: Viewer Password Incorrect.", 0); }

            if (command.accessLevel == 2 && config.AllowCommands)
            {
                if (command.command.ToLower() == "broadcast") { msg = CommandHandler.Broadcast(command.args); commandType = "broadcast"; success = 1; }
                else if (command.command.ToLower() == "ban") { msg = CommandHandler.Ban(command.args); commandType = "ban"; success = 1; }
                else if (command.command.ToLower() == "kick") { msg = CommandHandler.Kick(command.args); commandType = "kick"; success = 1; }
            }
            else if (!gotResponse && config.AllowCommands) { return new ReturnJson("error", "Error: Admin Password incorrect.", 0); }
            else if (!config.AllowCommands) { return new ReturnJson("error", "Error: Admin Commands are disabled in the config.", 0);  }



            return new ReturnJson(commandType, msg, success);
        }
    }

    public class CommandJson
    {
        public string command { get; set; }
        public string[] args { get; set; }
        public string password { get; set; }
        public int accessLevel;
        public CommandJson()
        {
        }

    }
    
}

public class ReturnJson
{
    public string commandType;
    public string result;
    public int success = 0;
    public ReturnJson(string _commandType, string _result, int _success)
    {
        commandType = _commandType;
        result = _result;
        success = _success;
    }
}
