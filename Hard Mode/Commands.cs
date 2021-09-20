using PulsarPluginLoader.Chat.Commands;

namespace Hard_Mode
{
    class Commands : IChatCommand
    {
        public string[] CommandAliases()
        {
            return new string[]
            {
                "hm",
                "hdm",
                "hardmode"
            };
        }
        public string Description()
        {
            return $"Configurations for Hard Mode";
        }
        public string UsageExample()
        {
            return "/" + this.CommandAliases()[0] + "(subcommand)";
        }
        public bool PublicCommand()
        {
            return false;
        }
        public bool Execute(string arguments, int SenderID)
        {
            if (!PhotonNetwork.isMasterClient) 
            {
                PLServer.Instance.AddNotification("Must be Host to use this command!", PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 2000, false);
                return false;
            }
            string[] argument = arguments.Split(' ');
            switch (argument[0].ToLower()) 
            {
                default:
                    PulsarPluginLoader.Utilities.Messaging.Echo(PLNetworkManager.Instance.LocalPlayer, "Avaliable options (type it all with no spaces): FogofWar, DangerousReactor, Weakreactor");
                    break;
                case "fog":
                case "fow":
                case "fogofwar":
                    Options.FogOfWar = !Options.FogOfWar;
                    PLServer.Instance.AddNotification("Fog of War " + (Options.FogOfWar ? "Enabled" : "Disabled"), PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 3000, false);
                    break;
                case "dr":
                case "dangerousreactor":
                    Options.DangerousReactor = !Options.DangerousReactor;
                    PLServer.Instance.AddNotification("Dangerous Reactor " + (Options.DangerousReactor ? "Enabled" : "Disabled"), PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 3000, false);
                    break;
                case "wr":
                case "weakreactor":
                    Options.WeakReactor = !Options.WeakReactor;
                    PLServer.Instance.AddNotification("Weak Reactors " + (Options.WeakReactor ? "Enabled" : "Disabled"), PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 3000, false);
                    break;
            }
            return false;
        }
    }
}
