using PulsarModLoader.Chat.Commands.CommandRouter;

namespace Hard_Mode
{
    class Commands : ChatCommand
    {
        public override string[] CommandAliases()
        {
            return new string[]
            {
                "hm",
                "hdm",
                "hardmode"
            };
        }
        public override string Description()
        {
            return $"Configurations for Hard Mode";
        }

        public override string[][] Arguments()
        {
            return new string[][] { new string[] { "fog", "fow", "fogofwar", "dr", "dangerousreactor", "wr", "weakreactor" } };
        }
        public string UsageExample()
        {
            return "/" + this.CommandAliases()[0] + "(subcommand)";
        }
        public override void Execute(string arguments)
        {
            if (!PhotonNetwork.isMasterClient) 
            {
                PLServer.Instance.AddNotification("Must be Host to use this command!", PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 2000, false);
                return;
            }
            string[] argument = arguments.Split(' ');
            switch (argument[0].ToLower()) 
            {
                default:
                    PulsarModLoader.Utilities.Messaging.Echo(PLNetworkManager.Instance.LocalPlayer,"Avaliable options (type it all with no spaces): FogofWar, DangerousReactor");
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
        }
    }
}
