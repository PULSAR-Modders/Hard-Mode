using PulsarModLoader;
[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksTo("Assembly-CSharp")]
namespace Hard_Mode
{
    public class Mod : PulsarMod
    {

        public override string Version => "Alpha1.2";

        public override string Author => "EngBot, Pokegustavo, Mest, Craziness924, 18107";

        public override string ShortDescription => "Makes your game a nightmare";

        public override string Name => "HardMode";

        public override string HarmonyIdentifier()
        {
            return "modders.hardmode";
        }
    }
}
