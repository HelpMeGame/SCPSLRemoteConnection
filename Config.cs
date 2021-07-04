using System.ComponentModel;
using Exiled.API.Interfaces;

namespace RemoteConnection
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Port to allow connections through")]
        public int Port { get; set; } = 886;

        [Description("Password for basic level information like Player List and Uptime. Leave empty for no password.")]
        public string ViewerPassword { get; set; } = "Viewer_Password";

        [Description("Password for advanced level information like Roles, User ID's and command execution.")]
        public string AdminPassword { get; set; } = "SET_TO_STRONG_PASSWORD";

        [Description("Allow commands to be executed remotely by users with the Admin Password.")]
        public bool AllowCommands { get; set; } = false;
    }
}
