using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader.Chat.Commands.CommandRouter;
using PulsarModLoader.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Hard_Mode
{
#if DEBUG
    class SpawnHunter : ChatCommand
    {
        private static FieldInfo m_ActiveBountyHunter_TypeIDInfo = AccessTools.Field(typeof(PLServer), "m_ActiveBountyHunter_TypeID");
        private static FieldInfo m_ActiveBountyHunter_SectorIDInfo = AccessTools.Field(typeof(PLServer), "m_ActiveBountyHunter_SectorID");
        private static FieldInfo m_ActiveBountyHunter_SecondsSinceWarpInfo = AccessTools.Field(typeof(PLServer), "m_ActiveBountyHunter_SecondsSinceWarp");
        public override string[] CommandAliases() => new string[] { "spawnhunter" };
        public override string Description() => "Debug";
        public override void Execute(string arguments)
        {
            PLSectorInfo plsectorInfo2 = PLServer.GetCurrentSector();
            if (plsectorInfo2 != null)
            {
                /* Useful for setting a local ship to be a hunter
                float lowerBound = 0.85f;
                float higherBound = 1.2f;
                PLEncounterManager.ShipLayout randomPossibleHunterLayout = PLEncounterManager.Instance.GetRandomPossibleHunterLayout(lowerBound, higherBound);
                PLServer.Instance.BountyHunterLayout = randomPossibleHunterLayout;
                PLServer.Instance.BountyHunterInfo = new PLPersistantShipInfo(PLServer.Instance.BountyHunterLayout.ShipType, 6, plsectorInfo2, 0, isDestroyed: false, isFlagged: true);
                m_ActiveBountyHunter_TypeIDInfo.SetValue(PLServer.Instance, (ObscuredInt)0);
                m_ActiveBountyHunter_SectorIDInfo.SetValue(PLServer.Instance, (ObscuredInt)plsectorInfo2.ID);
                m_ActiveBountyHunter_SecondsSinceWarpInfo.SetValue(PLServer.Instance, (ObscuredFloat)0f);*/
                PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[]
                {
                                    "Bounty Hunter Spawned",
                                    Color.red,
                                    0,
                                    "[SHIP]"
                });
            }
            PLServer.Instance.SpawnHunter();
        }
    }
#endif
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
            return new string[][] { new string[] { "FogofWar", "DangerousReactor", "Weakreactor","SpinningCypher" , "AdvancedCloak" } };
        }
        private static FieldInfo cachedAIData = AccessTools.Field(typeof(PLPlayer), "cachedAIData");
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
                case "Aidebug":
                    string result = "switch(classID)\n{";
                    PLPlayer[] bots = new PLPlayer[4];
                    foreach (PLPlayer player in PLServer.Instance.AllPlayers) 
                    {
                        if (player.StartingShip == PLEncounterManager.Instance.PlayerShip && player.GetClassID() != 0) 
                        {
                            bots[player.GetClassID() - 1] = player;
                        }
                    }
                    int j = 0;
                    for(int i = 0; i < 4; i++) 
                    {
                        Dictionary<AIPriority, string> storedPriorites = new Dictionary<AIPriority, string>();
                        result += "case " + (i + 1) + ":\n";
                        foreach(AIPriority aIPriority in ((AIDataIndividual)cachedAIData.GetValue(bots[i])).Priorities) 
                        {
                            result += $"AIPriority aipriority{i}{j} = new AIPriority(AIPriorityType.{aIPriority.Type}, {aIPriority.TypeData}, {aIPriority.BasePriority});\n";
                            result += $"dataInv.Priorities.Add(aipriority{i}{j});\n";
                            foreach (PLAIPriorityOverride @override in aIPriority.Overrides) 
                            {
                                if (@override.OverrideType != EPriorityOverrideType.E_CAPTAINS_ORDER)
                                {
                                    result += $"aipriority{i}{j}.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.{@override.OverrideType}, {@override.OverrideSubID}, {@override.Priority}, {@override.Inverted.ToString().ToLower()}, {@override.Data}));\n";
                                }
                                else 
                                {
                                    result += "if(!enemyAI)\n{";
                                    result += $"aipriority{i}{j}.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.{@override.OverrideType}, {@override.OverrideSubID}, {@override.Priority}, {@override.Inverted.ToString().ToLower()}, {@override.Data}));\n";
                                    result += "}\n";
                                }
                            }
                            foreach (PLPriorityMetadata metadata in aIPriority.Metadata) 
                            {
                                if (metadata is PLPriorityMetadata_Float)
                                {
                                    result += $"aipriority{i}{j}.Metadata.Add(new PLPriorityMetadata_Float({(metadata as PLPriorityMetadata_Float).Data}, {(metadata as PLPriorityMetadata_Float).StepSize}, \"{(metadata as PLPriorityMetadata_Float).Name}\"));\n";
                                }
                                else if (metadata is PLPriorityMetadata_FloatRange)
                                {
                                    result += $"aipriority{i}{j}.Metadata.Add(new PLPriorityMetadata_FloatRange({(metadata as PLPriorityMetadata_FloatRange).Data},{(metadata as PLPriorityMetadata_FloatRange).Min},{(metadata as PLPriorityMetadata_FloatRange).Max}, {(metadata as PLPriorityMetadata_FloatRange).StepSize}));\n";
                                }
                                else if (metadata is PLPriorityMetadata_Toggle) 
                                {
                                    result += $"aipriority{i}{j}.Metadata.Add(new PLPriorityMetadata_Toggle({(metadata as PLPriorityMetadata_Toggle).On}));\n";
                                }
                                else if (metadata is PLPriorityMetadata_ProgramPriorityArray)
                                {
                                    ExitGames.Client.Photon.Hashtable table;
                                    metadata.NetworkData_Init(PLServer.Instance, out table);
                                    result += $"aipriority{i}{j}.Metadata.Add(new PLPriorityMetadata_ProgramPriorityArray({table}));\n";
                                }
                            }
                            j++;
                            storedPriorites.Add(aIPriority, $"aipriority{i}{j}");
                        }
                        foreach (KeyValuePair<AIPriority, string> origin in storedPriorites)
                        {
                            string father = origin.Value;
                            foreach (AIPriority subPriority in origin.Key.Subpriorities)
                            {
                                foreach (KeyValuePair<AIPriority,string> data in storedPriorites)
                                {
                                    if (data.Key == subPriority) 
                                    {
                                        result += $"{father}.Subpriorities.Add({data.Value});\n";
                                    }
                                }
                            }
                        }
                        result += "break;\n";
                        j = 0;
                        
                    }
                    result += "}";
                    PulsarModLoader.Utilities.Logger.Info(result);
                    Messaging.Notification("AI data generator collected");
                    break;
                case "spinningcypher":
                case "sc":
                    Options.SpinningCycpher = !Options.SpinningCycpher;
                    Messaging.Notification("Spinning Cyphers " + (Options.SpinningCycpher ? "Enabled" : "Disabled"), (PLPlayer)null, 0, 3000);

                    break;
                case "advancedcloak":
                case "cloak":
                    Options.AdvancedCloak = !Options.AdvancedCloak;
                    Messaging.Notification("Advanced Cloak " + (Options.AdvancedCloak ? "Enabled" : "Disabled"), (PLPlayer)null, 0, 3000);
                    break;
            }
        }
    }
}
