using HarmonyLib;
using PulsarPluginLoader;

namespace Hard_Mode
{
    class Options //For all future options
    {
        public static bool FogOfWar = false;
        public static bool DangerousReactor = false;
        public static bool MasterHasMod = false;
    }

    class ReciveOptions : ModMessage //Recive the options from the master client
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            Options.FogOfWar = (bool)arguments[0];
            Options.DangerousReactor = (bool)arguments[1];
            Options.MasterHasMod = (bool)arguments[2];
        }
    }

    [HarmonyPatch(typeof(PLGlobal), "EnterNewGame")]
    class OnJoin //This should reset the configs everytime you enter a game and is not the host, so you don't end up with the effects in not modded games
    {
        static void Postfix()
        {
            if (!PhotonNetwork.isMasterClient) 
            {
                Options.FogOfWar = false;
                Options.DangerousReactor = false;
                Options.MasterHasMod = false;
            }
            else 
            {
                Options.MasterHasMod = true;
            }
        }
    }
}
