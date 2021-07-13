using PulsarPluginLoader;
namespace Hard_Mode
{
    public class Plugin : PulsarPlugin
    {

        public override string Version => "0.0";

        public override string Author => "EngBot, Pokegustavo, Mest";

        public override string ShortDescription => "Makes your game a nightmare";

        public override string Name => "HardMode";

        public override string HarmonyIdentifier()
        {
            return "modders.hardmode";
        }
    }
}
