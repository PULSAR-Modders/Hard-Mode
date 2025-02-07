using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using System.Reflection.Emit;
using PulsarModLoader.Patches;
using UnityEngine;
using static PulsarModLoader.Patches.HarmonyHelpers;
using System.Linq;
using PulsarModLoader.Utilities;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using ExitGames.Demos.DemoAnimator;

namespace Hard_Mode
{
    class Custom_Bounty_Hunters
    {
        /// <summary>
        /// Used for custom Pawn Hunters
        /// </summary>
        internal class Custom_Pawn
        {
            private static FieldInfo m_ActiveBountyHunter_TypeIDInfo = AccessTools.Field(typeof(PLServer), "m_ActiveBountyHunter_TypeID");
            private static bool __initial;
            private static string __name;
            private static int __classid;
            private static CustomPawnData __appearance;
            private static PLPlayer __player;
            private static PLShipInfoBase __hunterShip;
            [HarmonyPatch(typeof(PLSpawner), "DoSpawn")]
            public class HunterRespawnPatch
            { // Do Hunter respawn
                static bool Prefix(PLSpawner __instance)
                {
                    if (__instance.Spawn != "Hunter") return true;
                    PLServer server = PLServer.Instance;
                    if (!__initial && (__hunterShip == null || __hunterShip.HasBeenDestroyed))
                    {
                        __instance.ShouldRespawnEnemy = false;
                        __instance.enabled = false;
                        return false;
                    }
                    PLSpawner.DoSpawnStatic(PLEncounterManager.Instance.GetCPEI(), "Bandit", PLEncounterManager.Instance.PlayerShip.MyTLI.AllTTIs[0].transform, __instance, PLEncounterManager.Instance.PlayerShip.MyTLI, null, null);
                    PLPlayer component = PLEncounterManager.Instance.GetCPEI().MyCreatedPlayers[PLEncounterManager.Instance.GetCPEI().MyCreatedPlayers.Count - 1].GetComponent<PLPlayer>();
                    __player = component;
                    component.SetClassID(__classid);
                    component.GetPawn().BlockWarpWhenOnboard = true;
                    if (server.MyHunterSpawner_RaceParameter.Value == "0")
                    {
                        component.GetPawn().SetExosuitIsActive(true);
                    }
                    component.SetPlayerName(__name);
                    component.Talents[56] = Mathf.RoundToInt(5f + server.ChaosLevel * 2f);
                    component.Talents[0] = Mathf.RoundToInt(20f + server.ChaosLevel * 4f);
                    int num;
                    var m_ActiveBountyHunter_TypeID = (ObscuredInt)m_ActiveBountyHunter_TypeIDInfo.GetValue(server);
                    if (m_ActiveBountyHunter_TypeID == 0)
                    {
                        PLPawnInventoryBase myInventory = component.MyInventory;
                        PLServer instance = PLServer.Instance;
                        num = instance.PawnInvItemIDCounter;
                        instance.PawnInvItemIDCounter = num + 1;
                        myInventory.UpdateItem(num, 7, 0, 4, 6);
                    }
                    else if (m_ActiveBountyHunter_TypeID == 2)
                    {
                        PLPawnInventoryBase myInventory2 = component.MyInventory;
                        PLServer instance2 = PLServer.Instance;
                        num = instance2.PawnInvItemIDCounter;
                        instance2.PawnInvItemIDCounter = num + 1;
                        myInventory2.UpdateItem(num, 30, 0, 0, 6);
                    }
                    else
                    {
                        PLPawnInventoryBase myInventory3 = component.MyInventory;
                        PLServer instance3 = PLServer.Instance;
                        num = instance3.PawnInvItemIDCounter;
                        instance3.PawnInvItemIDCounter = num + 1;
                        myInventory3.UpdateItem(num, 8, 0, 4, 6);
                    }
                    PLPawnInventoryBase myInventory4 = component.MyInventory;
                    PLServer instance4 = PLServer.Instance;
                    num = instance4.PawnInvItemIDCounter;
                    instance4.PawnInvItemIDCounter = num + 1;
                    myInventory4.UpdateItem(num, 31, 0, 0, -1);
                    PLPawnInventoryBase myInventory5 = component.MyInventory;
                    PLServer instance5 = PLServer.Instance;
                    num = instance5.PawnInvItemIDCounter;
                    instance5.PawnInvItemIDCounter = num + 1;
                    myInventory5.UpdateItem(num, 31, 0, 0, -1);
                    //base.StartCoroutine(this.ControlledSEND_SetupNewHunter(component.GetPawn()));
                    return false;
                }
            }

            [HarmonyPatch(typeof(PLServer), "SpawnHunterPawn")]
            public class SpawnPatch
            { // Initialize respawning
                static bool Prefix(PLServer __instance)
                {
                    if (__instance.MyHunterSpawner == null)
                    {
                        __instance.MyHunterSpawner = __instance.gameObject.AddMissingComponent<PLSpawner>();
                        __instance.MyHunterSpawner.Spawn = "Hunter";
                        __instance.MyHunterSpawner.ShouldRespawnEnemy = true;
                        __instance.MyHunterSpawner.SpawnedEnemyRespawn_NoPlayersWithinMeters = 0f;
                        __instance.MyHunterSpawner.SpawnedEnemyRespawnTime_Seconds = Mathf.RoundToInt(30f - (__instance.ChaosLevel * 15f / 9f));
                        __instance.MyHunterSpawner_FactionParameter = new SpawnParameter
                        {
                            Name = "Faction"
                        };
                        __instance.MyHunterSpawner_RaceParameter = new SpawnParameter
                        {
                            Name = "Race"
                        };
                        __instance.MyHunterSpawner_GenderParameter = new SpawnParameter
                        {
                            Name = "Gender"
                        };
                        __instance.MyHunterSpawner.Parameters.Add(__instance.MyHunterSpawner_FactionParameter);
                        __instance.MyHunterSpawner.Parameters.Add(__instance.MyHunterSpawner_RaceParameter);
                        __instance.MyHunterSpawner.Parameters.Add(__instance.MyHunterSpawner_GenderParameter);
                        __instance.MyHunterSpawner.enabled = true;
                    }
                    var m_ActiveBountyHunter_TypeID = (ObscuredInt)m_ActiveBountyHunter_TypeIDInfo.GetValue(__instance);
                    if (m_ActiveBountyHunter_TypeID == 0)
                    {
                        __instance.MyHunterSpawner_FactionParameter.Value = "0";
                        __instance.MyHunterSpawner_RaceParameter.Value = UnityEngine.Random.Range(0, 3).ToString();
                    }
                    else if (m_ActiveBountyHunter_TypeID == 2)
                    {
                        __instance.MyHunterSpawner_FactionParameter.Value = "2";
                        __instance.MyHunterSpawner_RaceParameter.Value = UnityEngine.Random.Range(0, 3).ToString();
                    }
                    else
                    {
                        __instance.MyHunterSpawner_FactionParameter.Value = "1";
                        __instance.MyHunterSpawner_RaceParameter.Value = UnityEngine.Random.Range(0, 3).ToString();
                    }
                    if (__instance.MyHunterSpawner_RaceParameter.Value != "0")
                    {
                        __instance.MyHunterSpawner_GenderParameter.Value = "male";
                    }
                    else
                    {
                        __instance.MyHunterSpawner_GenderParameter.Value = ((UnityEngine.Random.value < 0.5f) ? "male" : "female");
                    }
                    __classid = UnityEngine.Random.Range(0, 5);
                    string text = PLServer.Instance.NPCNames[UnityEngine.Random.Range(0, PLServer.Instance.NPCNames.Count)];
                    if (text.Contains(" "))
                    {
                        text = text.Split(new char[] { ' ' })[0];
                    }
                    __name = text + " The Hunter";
                    __initial = true;
                    __instance.MyHunterSpawner.DoSpawn(PLEncounterManager.Instance.GetCPEI());
                    // Original spawn code moved to respawn method (Above) to reduce repetitions
                    return false;
                }
            }
            [HarmonyPatch(typeof(PLPlayer), "RandomizeCustomPawnData")]
            public class AppearancePatch
            { // Consistent Appearance
                static void Postfix(PLPlayer __instance, PLCustomPawn inPawn, CustomPawnData inData, bool enforceUnlocks = true)
                {
                    if (__player != __instance) return;
                    if (__initial)
                    {
                        __appearance = inData;
                        __initial = false;
                    }
                    else
                    {
                        __appearance.CopyTo(__instance.MyCustomPawnData[__instance.GetPawnCosmeticType()]);
                    }
                }
            }
            [HarmonyPatch(typeof(PLPersistantEncounterInstance), "SpawnEnemyShip")]
            public class PatchHunterPawnShip
            { // Used for respawning mechanic, to stop respawning when hunter ship is dead
                static void Postfix(EShipType inEnemyShipType, ref PLShipInfoBase __result)
                {
                    if (inEnemyShipType == EShipType.E_BOUNTY_HUNTER_01) __hunterShip = __result;
                }
            }
        }
        [HarmonyPatch(typeof(PLEncounterManager), "Start")]
        class HunterAdder //Adds the extra hunters from the HunterCodes.txt for the UnityEngine.Random list when the game starts 
        {
            static void Postfix(ref List<PLEncounterManager.ShipLayout> ___PossibleHunters_LayoutData)
            {
                string path = Path.Combine(AppContext.BaseDirectory, "Mods\\HunterCodes.txt");
                if (!File.Exists(path))
                {
                    return;
                }
                using (StreamReader streamReader = new StreamReader(path)) //This reads the file
                {
                    while (!streamReader.EndOfStream)
                    {
                        string bountyhunter = streamReader.ReadLine();
                        if (bountyhunter.Length > 50)
                        {
                            PLEncounterManager.ShipLayout hunter = new PLEncounterManager.ShipLayout(bountyhunter);
                            if (hunter.ShipType == EShipType.E_INTREPID_SC) hunter.ShipType = EShipType.E_INTREPID;
                            if (hunter.ShipType == EShipType.E_ALCHEMIST) hunter.ShipType = EShipType.E_CARRIER;
                            ___PossibleHunters_LayoutData.Add(hunter);
                        }
                    }
                }
            }
        }
        public static bool updated = false;
        public static void UpdateHunterLevel() //This updates the combat level to the system we use
        {
            if (PLServer.GetCurrentSector() != null)
            {
                List<PLEncounterManager.ShipLayout> _PossibleHunters_LayoutData = PLEncounterManager.Instance.GetAllLayouts();
                foreach (PLEncounterManager.ShipLayout ship in _PossibleHunters_LayoutData)
                {
                    /*
                    List<PLShipComponent> allComponents = new List<PLShipComponent>();
                    PLHull hull = null;
                    PLShieldGenerator shield = null;
                    PLReactor reactor = null;
                    List<PLThruster> thrusters = new List<PLThruster>();
                    */
                    BinaryReader binaryReader = new BinaryReader(new MemoryStream(System.Convert.FromBase64String(ship.Data)));
                    binaryReader.ReadInt32();
                    binaryReader.ReadSingle();
                    while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                    {
                        int inHash = binaryReader.ReadInt32();
                        PLShipComponent plshipComponent = PLWare.CreateFromHash(1, inHash) as PLShipComponent;
                        if (plshipComponent != null && (plshipComponent.ActualSlotType == ESlotType.E_COMP_AUTO_TURRET || plshipComponent.ActualSlotType == ESlotType.E_COMP_MAINTURRET || plshipComponent.ActualSlotType == ESlotType.E_COMP_TURRET))
                        {
                            PLTurret turret = plshipComponent as PLTurret;
                            CombatLevel.GetTurretCombatLevel(turret, ref ship.CL);
                            /*
                            allComponents.Add(plshipComponent);
                            switch (plshipComponent.ActualSlotType)
                            {
                                case ESlotType.E_COMP_REACTOR:
                                    reactor = plshipComponent as PLReactor;
                                    break;
                                case ESlotType.E_COMP_HULL:
                                    hull = plshipComponent as PLHull;
                                    break;
                                case ESlotType.E_COMP_SHLD:
                                    shield = plshipComponent as PLShieldGenerator;
                                    break;
                                case ESlotType.E_COMP_THRUSTER:
                                    thrusters.Add(plshipComponent as PLThruster);
                                    break;
                            }
                            */
                        }
                    }
                    /*
                    ship.CL = 0;
                    foreach (PLShipComponent plshipComponent in allComponents)
                    {
                        ship.CL += Mathf.Pow((float)plshipComponent.GetScaledMarketPrice(true), 0.8f) * 0.001f;
                        if (plshipComponent.ActualSlotType == ESlotType.E_COMP_MAINTURRET || plshipComponent.ActualSlotType == ESlotType.E_COMP_TURRET || plshipComponent.ActualSlotType == ESlotType.E_COMP_AUTO_TURRET)
                        {
                            PLTurret turret = plshipComponent as PLTurret;
                            ship.CL += turret.GetDPS() * 0.1f * (1f - Mathf.Clamp01(turret.HeatGeneratedOnFire * 2.2f / turret.FireDelay));
                            ship.CL += turret.TurretRange * 0.0005f;
                        }
                    }
                    if (hull != null)
                    {
                        ship.CL += hull.Max * hull.LevelMultiplier(0.2f, 1f) * 0.005f;
                        ship.CL += hull.Armor * hull.LevelMultiplier(0.15f, 1f) * 10f * (ship.ShipType == EShipType.E_DESTROYER ? 1.5f : 1);
                    }
                    if (shield != null)
                    {
                    }
                    PLPersistantShipInfo plpersistantShipInfo = new PLPersistantShipInfo(ship.ShipType, -1, PLServer.GetCurrentSector(), 0, false, false, false, -1, -1);
                    PulsarPluginLoader.Utilities.Logger.Info("Ship: ");
                    plpersistantShipInfo.CreateShipInstance(PLEncounterManager.Instance.GetCPEI());
                    plpersistantShipInfo.ShipInstance.MyStats.FormatToDataString(ship.Data);
                    plpersistantShipInfo.ShipInstance.MyStats.CalculateStats();
                    ship.CL = plpersistantShipInfo.ShipInstance.GetCombatLevel();
                    PhotonNetwork.Destroy(plpersistantShipInfo.ShipInstance.photonView);
                    */
                }
                updated = true;
            }
        }
        [HarmonyPatch(typeof(PLPersistantEncounterInstance), "PlayerEnter")]
        public class RelicHunterBalance //Allow to change the Min and Max values for the difference between you and the relic hunter
        {
            static public float MinCombatLevel = 1.2f;
            static public float MaxCombatLevel = 1.5f;
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLEncounterManager),"Instance")),
                new CodeInstruction(OpCodes.Ldc_R4,1.2f),
                new CodeInstruction(OpCodes.Ldc_R4,1.5f)
            };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLEncounterManager),"Instance")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(RelicHunterBalance), "MinCombatLevel")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(RelicHunterBalance), "MaxCombatLevel"))
            };
                return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }
        [HarmonyPatch(typeof(PLPersistantEncounterInstance), "SpawnRelevantRelicHunter")]
        class UpdateRelic //This Updates the combat levels for relic hunter
        {
            static void Prefix() 
            {
                if (!updated) UpdateHunterLevel();
            }
        }
        [HarmonyPatch(typeof(PLServer), "SpawnHunter")]
        public class BountyHunterBalance //Allow to change the Min and Max values for the difference between you and the bounty hunter
        {
            static public float MinCombatLevel = 1.2f;
            static public float MaxCombatLevel = 1.5f;
            static void Prefix()
            {
                if (!updated) UpdateHunterLevel();
            }
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLEncounterManager),"Instance")),
                new CodeInstruction(OpCodes.Ldloc_3),
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLEncounterManager),"GetRandomPossibleHunterLayout")),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLEncounterManager),"Instance")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(BountyHunterBalance), "MinCombatLevel")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(BountyHunterBalance), "MaxCombatLevel")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLEncounterManager),"GetRandomPossibleHunterLayout")),
                };
                patchSequence[0].labels = instructions.ToList()[FindSequence(instructions, targetSequence, CheckMode.NONNULL) - 4].labels;
                return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.NONNULL, false);
            }
        }
        [HarmonyPatch(typeof(PLPersistantEncounterInstance), "SpawnEnemyShip")]
        class BountyHunterName //This is so bounty hunters in ships from fluffy company or from no faction to have a name instead of N/A
        {
            static void Postfix(ref PLShipInfoBase __result)
            {
                PLShipInfo ship = __result as PLShipInfo;
                if (ship != null)
                {
                    if (ship.ShipTypeID == EShipType.E_INTREPID_SC || ship.ShipTypeID == EShipType.E_ALCHEMIST || ship.IsDrone || __result.PersistantShipInfo.MyCurrentSector.MissionSpecificID != -1) return;
                    switch (ship.FactionID)
                    {
                        case -1:
                            ship.ShipNameValue = PLServer.Instance.AOGShipNameGenerator.GetName(PLServer.Instance.GalaxySeed + ship.ShipID);
                            break;
                        case 3:
                            string[] biscuitnames = new string[]
                            {
                            "Muffin Mashers",
                            "Crispy Cruiser",
                            "Tasty Toasters",
                            "Delivery Dogs",
                            "Hunger Hinderers",
                            "Bombastic Bites",
                            "Goodstuff Foodstuff",
                            "Merry Meals",
                            "Fasting Foodies",
                            "Sugar Speeders",
                            "Generic Grillers",
                            "Ham Hoarders",
                            "Ration Replacers",
                            "Rapid Responders",
                            "Mad Merchants",
                            "Voracious Vendors",
                            "Palatable Peddlers",
                            "Wandering Wafer",
                            "Biscuit Boys",
                            "Necessary Nutritions",
                            "Hungry Hucksters",
                            "Flavor’s Fury",
                            "Tea Timers",
                            };
                            ship.ShipNameValue = biscuitnames[UnityEngine.Random.Range(0, biscuitnames.Length - 1)];
                            break;
                    }
                    if(ship.GetCombatLevel() > 135 && UnityEngine.Random.value < 0.1 && ship.PersistantShipInfo.ShipName != "") 
                    {
                        ship.ShipNameValue = "The Glass Revenant Mk " + UnityEngine.Random.Range(1, 999);
                    }
                }
            }
        }
    }
}
