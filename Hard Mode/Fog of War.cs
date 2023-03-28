using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace Hard_Mode
{
    public class Blindfolding
    {
        [HarmonyPatch(typeof(PLStarmap), "ShouldShowSectorBG")]
        class StarmapSectorDotRemoval // removes all normal sectors from the map if they aren't discovered or other conditions
        {
            static void Postfix(ref bool __result, PLSectorInfo sectorInfo)
            {
                if (!Options.FogOfWar)
                {
                    return;
                }
                int crewFactionID = PLServer.Instance.CrewFactionID;
                bool shouldshowestate = (crewFactionID == 1);
                if (sectorInfo.VisualIndication == ESectorVisualIndication.GENTLEMEN_START || sectorInfo.VisualIndication == ESectorVisualIndication.AOG_HUB)
                {
                    __result = (shouldshowestate || sectorInfo.Discovered);
                    return;
                }
                else if (sectorInfo.MySPI.Faction == crewFactionID && sectorInfo.MissionSpecificID == -1 && !shouldshowestate)
                {
                    __result = true;
                    return;
                }
                __result = (sectorInfo != null && sectorInfo.VisualIndication != ESectorVisualIndication.COMET && sectorInfo.VisualIndication != ESectorVisualIndication.TOPSEC && sectorInfo.VisualIndication != ESectorVisualIndication.LCWBATTLE && (sectorInfo.MissionSpecificID == -1 || PLServer.Instance.HasActiveMissionWithID(sectorInfo.MissionSpecificID)) && (sectorInfo.Visited || sectorInfo.Discovered || (((sectorInfo.Name != sectorInfo.ID.ToString() && !sectorInfo.Name.Contains("Sys") && !sectorInfo.Name.Contains("Karattis")) || sectorInfo.VisualIndication == ESectorVisualIndication.AOG_MISSIONCHAIN_PRISONBREAK || sectorInfo.VisualIndication == ESectorVisualIndication.RACING_SECTOR_3 || sectorInfo.VisualIndication == ESectorVisualIndication.RACING_SECTOR_2 || sectorInfo.VisualIndication == ESectorVisualIndication.RACING_SECTOR || sectorInfo.VisualIndication == ESectorVisualIndication.ALCHEMIST || sectorInfo.VisualIndication == ESectorVisualIndication.DESERT_HUB || sectorInfo.VisualIndication == ESectorVisualIndication.SWARM_CMDR || sectorInfo.VisualIndication == ESectorVisualIndication.DEATHSEEKER_COMMANDER || sectorInfo.VisualIndication == ESectorVisualIndication.INTREPID_SECTOR_CMDR || sectorInfo.VisualIndication == ESectorVisualIndication.SPACE_SCRAPYARD || sectorInfo.VisualIndication == ESectorVisualIndication.AOG_HUB || sectorInfo.VisualIndication == ESectorVisualIndication.ANCIENT_SENTRY || sectorInfo.VisualIndication == ESectorVisualIndication.GENERAL_STORE || /* sectorInfo.VisualIndication == ESectorVisualIndication.EXOTIC1 || sectorInfo.VisualIndication == ESectorVisualIndication.EXOTIC2 || sectorInfo.VisualIndication == ESectorVisualIndication.EXOTIC3 || */ sectorInfo.VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION || PLServer.Instance.m_ShipCourseGoals.Contains(sectorInfo.ID) || sectorInfo.MissionSpecificID != -1) && (sectorInfo.MissionSpecificID == -1 || PLServer.Instance.HasActiveMissionWithID(sectorInfo.MissionSpecificID)) && sectorInfo.VisualIndication != ESectorVisualIndication.COMET)));

                if (OnCreation.shouldrandom && !PLNetworkManager.Instance.MyEncounterManager.IsInPreGame && PLGlobal.Instance != null && PLGlobal.Instance.Galaxy != null && PhotonNetwork.isMasterClient) // Give random sectors from the galaxy
                {
                    OnCreation.shouldrandom = false;
                    if (PLServer.Instance.CrewFactionID != 0 && PLServer.Instance.CrewFactionID != 2)
                    {
                        double i = 0;
                        while (i < PLGlobal.Instance.Galaxy.AllSectorInfos.Count * 0.3)
                        {
                            PLSectorInfo sector;
                            bool exists = PLGlobal.Instance.Galaxy.AllSectorInfos.TryGetValue(UnityEngine.Random.Range(0, PLGlobal.Instance.Galaxy.AllSectorInfos.Count * 2), out sector);
                            if (exists && !sector.Discovered && sector.MissionSpecificID == -1)
                            {
                                foreach (PLSectorInfo pLSectorInfo in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                                {
                                    if (pLSectorInfo.ID == sector.ID)
                                    {
                                        pLSectorInfo.Discovered = true;
                                        i++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        [HarmonyPatch(typeof(PLStarmap), "ShouldShowSector")]
        class StarmapSectorIconRemoval // removes sector icons (like a circle that says "The Estate" from the Starmap
        {
            static void Postfix(ref bool __result, PLSectorInfo sectorInfo)
            {
                if (!Options.FogOfWar)
                {
                    return;
                }
                bool isGWG = (sectorInfo.VisualIndication == ESectorVisualIndication.GWG) && (PLServer.Instance != null);
                int crewFactionID = PLServer.Instance.CrewFactionID;
                bool shouldshowestate = (crewFactionID == 1);
                if (sectorInfo.VisualIndication == ESectorVisualIndication.GENTLEMEN_START || sectorInfo.VisualIndication == ESectorVisualIndication.AOG_HUB)
                {
                    __result &= (shouldshowestate || sectorInfo.Discovered);
                    return;
                }
                __result &= ((sectorInfo.Name != sectorInfo.ID.ToString() && !sectorInfo.Name.Contains("Sys") && !sectorInfo.Name.Contains("Karattis")) || isGWG || sectorInfo.IsThisSectorWithinPlayerWarpRange() || sectorInfo.VisualIndication == ESectorVisualIndication.AOG_MISSIONCHAIN_PRISONBREAK || sectorInfo.VisualIndication == ESectorVisualIndication.RACING_SECTOR_3 || sectorInfo.VisualIndication == ESectorVisualIndication.RACING_SECTOR_2 || sectorInfo.VisualIndication == ESectorVisualIndication.RACING_SECTOR || sectorInfo.VisualIndication == ESectorVisualIndication.ALCHEMIST || sectorInfo.VisualIndication == ESectorVisualIndication.DESERT_HUB || sectorInfo.VisualIndication == ESectorVisualIndication.SWARM_CMDR || sectorInfo.VisualIndication == ESectorVisualIndication.DEATHSEEKER_COMMANDER || sectorInfo.VisualIndication == ESectorVisualIndication.INTREPID_SECTOR_CMDR || sectorInfo.VisualIndication == ESectorVisualIndication.SPACE_SCRAPYARD || sectorInfo.VisualIndication == ESectorVisualIndication.AOG_HUB || sectorInfo.VisualIndication == ESectorVisualIndication.ANCIENT_SENTRY || sectorInfo.VisualIndication == ESectorVisualIndication.GENERAL_STORE || sectorInfo.VisualIndication == ESectorVisualIndication.EXOTIC1 || sectorInfo.VisualIndication == ESectorVisualIndication.EXOTIC2 || sectorInfo.VisualIndication == ESectorVisualIndication.EXOTIC3 || sectorInfo.VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION || PLServer.Instance.m_ShipCourseGoals.Contains(sectorInfo.ID) || sectorInfo.MissionSpecificID != -1) && (sectorInfo.MissionSpecificID == -1 || PLServer.Instance.HasActiveMissionWithID(sectorInfo.MissionSpecificID)) && sectorInfo.VisualIndication != ESectorVisualIndication.COMET;

            }
        }
        [HarmonyPatch(typeof(PLNetworkManager), "OnServerCreatedRoom")]
        class OnCreation
        {
            public static bool shouldrandom = false;
            static void Postfix()
            {
                if (!PLNetworkManager.Instance.MyEncounterManager.IsInPreGame) shouldrandom = true;
            }
        }
        [HarmonyPatch(typeof(PLSaveGameIO), "SaveToFile")]
        class SaveDiscovered
        {
            static void Prefix()
            {
                foreach (PLSectorInfo sector in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                {
                    /*
                    if (sector.Discovered)
                    {
                        sector.Name += "­";
                    }
                    */
                    if (sector.Discovered)
                    {
                        switch (sector.MySPI.Faction)
                        {
                            case 0:
                                sector.FactionStrength = 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_CU + 1f;
                                break;
                            case 1:
                                sector.FactionStrength = 0.6f;
                                break;
                            case 2:
                                sector.FactionStrength = 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_WD + 1f;
                                break;
                            case 3:
                                sector.FactionStrength = 31f;
                                break;
                            case 4:
                                sector.FactionStrength = 200 * PLGlobal.Instance.Galaxy.GenerationSettings.InfectionInitialStrength + 1f;
                                break;
                        }
                    }
                }
            }

            static void Postfix()
            {
                foreach (PLSectorInfo sector in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                {/*
                    if (sector.Name.Contains("­"))
                    {
                       sector.Name = sector.Name.Remove(sector.Name.LastIndexOf('­'));
                    }
                  */
                    if (sector.FactionStrength == 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_CU + 1f)
                    {
                        sector.FactionStrength = 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_CU;
                    }
                    else if (sector.FactionStrength == 0.6f)
                    {
                        sector.FactionStrength = 0.5f;
                    }
                    else if (sector.FactionStrength == 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_WD + 1f)
                    {
                        sector.FactionStrength = 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_WD;
                    }
                    else if (sector.FactionStrength == 31f)
                    {
                        sector.FactionStrength = 30f;
                    }
                    else if (sector.FactionStrength == 200 * PLGlobal.Instance.Galaxy.GenerationSettings.InfectionInitialStrength + 1f)
                    {
                        sector.FactionStrength = 200 * PLGlobal.Instance.Galaxy.GenerationSettings.InfectionInitialStrength;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(PLGalaxy), "GetPathToSector_CustomRangeAndLimits")]
        class BountyHunterPath 
        {
            private static MethodInfo Heuristic = AccessTools.Method(typeof(PLGalaxy), "Heuristic");
            private static FieldInfo HWeight = AccessTools.Field(typeof(PLGalaxy), "HWeight");
            static void Postfix(PLSectorInfo inStartSector, PLSectorInfo inEndSector, float customWarpRange, bool onlyUseEmptySectors, ref List<PLSectorInfo> __result, PLGalaxy __instance)
            {
                if (PLServer.Instance.ActiveBountyHunter_TypeID != -1 && inStartSector.ID == PLServer.Instance.ActiveBountyHunter_SectorID && inEndSector == PLServer.GetCurrentSector())
                {
                    bool flag = false;
                    List<PLSectorInfo> list = new List<PLSectorInfo>();
                    List<PLSectorInfo> list2 = new List<PLSectorInfo>();
                    PLSectorInfo plsectorInfo = null;
                    float num = float.MaxValue;
                    foreach (PLSectorInfo plsectorInfo2 in __instance.AllSectorInfos.Values)
                    {
                        plsectorInfo2.Category = NodeCategory.NODE_DEF;
                        if (onlyUseEmptySectors && plsectorInfo2.VisualIndication == ESectorVisualIndication.NONE)
                        {
                            float num2 = Vector3.SqrMagnitude(inEndSector.Position - plsectorInfo2.Position);
                            if (num2 < num)
                            {
                                num = num2;
                                plsectorInfo = plsectorInfo2;
                            }
                        }
                    }
                    if (onlyUseEmptySectors)
                    {
                        inEndSector = plsectorInfo;
                    }
                    list.Clear();
                    list.Add(null);
                    list.Add(inStartSector);
                    inStartSector.FCost = 0f;
                    inStartSector.SearchParentNode = null;
                    PLSectorInfo plsectorInfo3 = null;
                    while (list.Count > 1 && !flag)
                    {
                        plsectorInfo3 = list[1];
                        plsectorInfo3.Category = NodeCategory.NODE_CLOSED;
                        list[1] = list[list.Count - 1];
                        list.RemoveAt(list.Count - 1);
                        int num3 = 1;
                        for (; ; )
                        {
                            int num4 = num3;
                            if (2 * num4 + 1 <= list.Count - 1)
                            {
                                if (list[num4].FCost >= list[2 * num4].FCost)
                                {
                                    num3 = 2 * num4;
                                }
                                if (list[num3].FCost >= list[2 * num4 + 1].FCost)
                                {
                                    num3 = 2 * num4 + 1;
                                }
                            }
                            else if (2 * num4 <= list.Count - 1 && list[num4].FCost >= list[2 * num4].FCost)
                            {
                                num3 = 2 * num4;
                            }
                            if (num4 == num3)
                            {
                                break;
                            }
                            PLSectorInfo value = list[num4];
                            list[num4] = list[num3];
                            list[num3] = value;
                        }
                        if (plsectorInfo3 == inEndSector)
                        {
                            flag = true;
                            break;
                        }
                        foreach (PLSectorInfo plsectorInfo4 in plsectorInfo3.Neighbors)
                        {
                            if ((!onlyUseEmptySectors || plsectorInfo4.VisualIndication == ESectorVisualIndication.NONE) && plsectorInfo4.Category != NodeCategory.NODE_CLOSED && (new Vector2(plsectorInfo3.Position.x, plsectorInfo3.Position.y) - new Vector2(plsectorInfo4.Position.x, plsectorInfo4.Position.y)).sqrMagnitude <= customWarpRange * customWarpRange)
                            {
                                float num5 = (float)Heuristic.Invoke(__instance, new object[] { plsectorInfo4, plsectorInfo3 });
                                float _HWeight = (float)HWeight.GetValue(__instance);
                                if (plsectorInfo4.Category == NodeCategory.NODE_DEF)
                                {
                                    if (!list.Contains(plsectorInfo4))
                                    {
                                        list.Add(plsectorInfo4);
                                    }
                                    plsectorInfo4.Category = NodeCategory.NODE_OPEN;
                                    plsectorInfo4.SearchParentNode = plsectorInfo3;
                                    float num6 = (float)Heuristic.Invoke(__instance, new object[] { plsectorInfo4, inEndSector });
                                    plsectorInfo4.GCost = plsectorInfo3.GCost + num6 * _HWeight;
                                    plsectorInfo4.HCost = num6 * _HWeight;
                                    plsectorInfo4.FCost = plsectorInfo4.GCost + plsectorInfo4.HCost * _HWeight;
                                    SortOpenList(ref list);
                                }
                                else if (plsectorInfo4.GCost > plsectorInfo3.GCost + num5)
                                {
                                    plsectorInfo4.GCost = plsectorInfo3.GCost + num5 * _HWeight;
                                    plsectorInfo4.FCost = plsectorInfo4.GCost + plsectorInfo4.HCost * _HWeight;
                                    plsectorInfo4.SearchParentNode = plsectorInfo3;
                                    SortOpenList(ref list);
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        PrepareSolution(ref list2, plsectorInfo3);
                        list2.Reverse();
                    }
                    __result = list2;
                }
            }
            // The following two methods were done as I dont know how to Invoke methods with 'ref'
            private static void SortOpenList(ref List<PLSectorInfo> OpenList)
            {
                int num2;
                for (int num = OpenList.Count - 1; num != 1; num = num2)
                {
                    num2 = num / 2;
                    if (OpenList[num].FCost > OpenList[num2].FCost)
                    {
                        break;
                    }
                    PLSectorInfo value = OpenList[num];
                    OpenList[num] = OpenList[num2];
                    OpenList[num2] = value;
                }
            }
            private static void PrepareSolution(ref List<PLSectorInfo> ReturnSolution, PLSectorInfo FromNode)
            {
                PLSectorInfo plsectorInfo = FromNode;
                ReturnSolution.Clear();
                if (plsectorInfo == null)
                {
                    return;
                }
                while (plsectorInfo.SearchParentNode != null)
                {
                    ReturnSolution.Add(plsectorInfo);
                    plsectorInfo = plsectorInfo.SearchParentNode;
                }
                ReturnSolution.Add(plsectorInfo);
            }
        }
    }
    [HarmonyPatch(typeof(PLServer), "ServerSetGalaxyFromFile")]
    class LoadDiscovered
    {
        static void Postfix()
        {
            foreach (PLSectorInfo sector in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
            {/*
                if (sector.Name.Contains("­"))
                {
                    sector.Discovered = true;
                    sector.Name = sector.Name.Remove(sector.Name.LastIndexOf('­'));
                }
              */
                if (sector.FactionStrength == 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_CU + 1f)
                {
                    sector.Discovered = true;
                    sector.FactionStrength = 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_CU;
                }
                else if (sector.FactionStrength == 0.6f)
                {
                    sector.Discovered = true;
                    sector.FactionStrength = 0.5f;
                }
                else if (sector.FactionStrength == 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_WD + 1f)
                {
                    sector.Discovered = true;
                    sector.FactionStrength = 5 * PLGlobal.Instance.Galaxy.GenerationSettings.FactionInitialStrengthScalar_WD;
                }
                else if (sector.FactionStrength == 31f)
                {
                    sector.Discovered = true;
                    sector.FactionStrength = 30f;
                }
                else if (sector.FactionStrength == 200 * PLGlobal.Instance.Galaxy.GenerationSettings.InfectionInitialStrength + 1f)
                {
                    sector.Discovered = true;
                    sector.FactionStrength = 200 * PLGlobal.Instance.Galaxy.GenerationSettings.InfectionInitialStrength;
                }

            }
        }
    }

}

