using PulsarModLoader.Chat.Commands.CommandRouter;
using PulsarModLoader.Utilities;

namespace Hard_Mode
{
    class Commands : ChatCommand
    {
        public override string[] CommandAliases()
        {
            return new string[]
            {
                "hardmode",
                "hdm",
                "hm"
            };
        }
        public override string Description()
        {
            return "Configurations for Hard Mode";
        }
        public override string[][] Arguments()
        {
            return new string[][] { new string[] { "FogofWar", "DangerousReactor", "Weakreactor" } };
        }
        public override void Execute(string arguments)
        {
            if (!PhotonNetwork.isMasterClient) 
            {
                Messaging.Notification("Must be Host to use this command!", (PLPlayer)null, 0, 2000);
                return;
            }
            string[] argument = arguments.Split(' ');
            switch (argument[0].ToLower()) 
            {
                default:
                    Messaging.Echo(PLNetworkManager.Instance.LocalPlayer, "Avaliable options (type it all with no spaces): FogofWar, DangerousReactor, Weakreactor");
                    break;
                case "fog":
                case "fow":
                case "fogofwar":
                    Options.FogOfWar = !Options.FogOfWar;
                    Messaging.Notification("Fog of War " + (Options.FogOfWar ? "Enabled" : "Disabled"), (PLPlayer)null, 0, 3000);
                    break;
                case "dr":
                case "dangerousreactor":
                    Options.DangerousReactor = !Options.DangerousReactor;
                    Messaging.Notification("Dangerous Reactor " + (Options.DangerousReactor ? "Enabled" : "Disabled"), (PLPlayer)null, 0, 3000);
                    break;
                case "wr":
                case "weakreactor":
                    Options.WeakReactor = !Options.WeakReactor;
                    Messaging.Notification("Weak Reactors " + (Options.WeakReactor ? "Enabled" : "Disabled"), (PLPlayer)null, 0, 3000);
                    break;
            }
        }
    }
}
