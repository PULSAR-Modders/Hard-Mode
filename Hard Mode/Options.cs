using HarmonyLib;
using PulsarModLoader;

namespace Hard_Mode
{
    class Options //For all future options
    {
        public static bool FogOfWar = false;
        public static bool DangerousReactor = false;
        public static bool MasterHasMod = false;
        public static bool WeakReactor = false;
        public static bool SpinningCycpher = false;
    }

    class ReciveOptions : ModMessage //Recive the options from the master client
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            Options.FogOfWar = (bool)arguments[0];
            Options.DangerousReactor = (bool)arguments[1];
            Options.MasterHasMod = (bool)arguments[2];
            Options.WeakReactor = (bool)arguments[3];
            Options.SpinningCycpher = (bool)arguments[4];
        }
    }
    [HarmonyPatch(typeof(PLServer), "ServerSendClientStarmap")]
    class UpdateMap //Updates the galaxy to the clients
    {
        static void Postfix(PhotonMessageInfo pmi) 
        {
            foreach (PLSectorInfo sector in PLGlobal.Instance.Galaxy.AllSectorInfos.Values) 
            {
                if (sector.Discovered)
                {
                    PLServer.Instance.photonView.RPC("ClientSectorInfo", pmi.sender, new object[]
                    {
                      sector.ID,
                      sector.Discovered,
                      sector.Visited
                    });
                }
            }
        }
    }

    [HarmonyPatch(typeof(PLGlobal), "EnterNewGame")]
    class OnJoin //This should reset the configs everytime you enter a game and is not the host, so you don't end up with the effects in not modded games
    {
        static void Postfix()
        {
            if (!PhotonNetwork.isMasterClient) 
            {
                Options.MasterHasMod = false;
                Options.FogOfWar = false;
                Options.DangerousReactor = false;
                Options.WeakReactor = false;
                Options.SpinningCycpher = false;
            }
            else 
            {
                Options.MasterHasMod = true;
            }
        }
    }
}
