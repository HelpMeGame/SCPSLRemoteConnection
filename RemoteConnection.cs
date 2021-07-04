using System;
using System.Threading;
using Exiled.API.Features;
using Exiled.API.Enums;

namespace RemoteConnection
{
    public class RemoteConnection : Plugin<Config>
    {
        public override string Author => "HelpMeGame";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(2, 8, 0);

        public static RemoteConnection singleton { get; } = new RemoteConnection();

        public override PluginPriority Priority { get; } = PluginPriority.Default;

        NetworkHandler networkHandler;
        Thread networkThread;

        private RemoteConnection(){}

        public override void OnEnabled()
        {
            networkHandler = new NetworkHandler();
            RegisterEvents();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            networkHandler = null;
        }

        public void RegisterEvents()
        {
            networkThread = new Thread(new ThreadStart(networkHandler.StartServer));
            networkThread.Start();
        }

        public void UnregisterEvents()
        {
            networkThread.Abort();
        }
    }
}
