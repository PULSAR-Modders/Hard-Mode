using PulsarPluginLoader;
[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksTo("Assembly-CSharp")]
namespace Hard_Mode
{
    public class Plugin : PulsarPlugin
    {

        public override string Version => "Alpha1.1";

        public override string Author => "EngBot, Pokegustavo, Mest, Craziness924";

        public override string ShortDescription => "Makes your game a nightmare";

        public override string Name => "HardMode";

        public override string HarmonyIdentifier()
        {
            return "modders.hardmode";
        }
    }
}
