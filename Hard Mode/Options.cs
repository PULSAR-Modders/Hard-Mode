using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.CustomGUI;
using UnityEngine;

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
    internal class Config : ModSettingsMenu
    {
        public override string Name() => "Hardmode Config";
        public override void Draw()
        {
            if (PLServer.Instance == null || PLNetworkManager.Instance == null || PLNetworkManager.Instance.LocalPlayer == null || !PLNetworkManager.Instance.LocalPlayer.GetHasStarted() || !Options.MasterHasMod)
            {
                GUILayout.Label("Hardmode Configuration requires you to be in a Hardmode game.");
                return;
            }
            if (!PhotonNetwork.isMasterClient)
            {
                GUILayout.Label("Must be HOST to change Hardmode Configuration!");
                return;
            }
            Options.FogOfWar = GUILayout.Toggle(Options.FogOfWar, "Fog Of War");
            GUILayout.Label("Hides undiscovered sectors from the map");
            Options.DangerousReactor = GUILayout.Toggle(Options.DangerousReactor, "Dangerous Reactors");
            GUILayout.Label("Increases the radiation range of the Reactors");
            Options.WeakReactor = GUILayout.Toggle(Options.WeakReactor, "Weak Reactors");
            GUILayout.Label("Reduces Reactor power output");
            Options.SpinningCycpher = GUILayout.Toggle(Options.SpinningCycpher, "Spinning Cyphers");
            GUILayout.Label("Makes Cyphers slowly spin");

        }
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
