using PulsarModLoader;
namespace Hard_Mode
{
    public class Mod : PulsarMod
    {

        public override string Version => "1.6.1";

        public override string Author => "EngBot, Pokegustavo, Mest, Craziness924, 18107, sugarbuzz1";

        public override string ShortDescription => "Makes your game a nightmare";

        public override string Name => "HardMode";

        public override string HarmonyIdentifier()
        {
            return "modders.hardmode";
        }


    }
}
