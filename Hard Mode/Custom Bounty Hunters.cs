using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using System.Reflection.Emit;
using PulsarModLoader.Patches;
using UnityEngine;
using static PulsarModLoader.Patches.HarmonyHelpers;
using System.Linq;

namespace Hard_Mode
{
    class Custom_Bounty_Hunters
    {
        [HarmonyPatch(typeof(PLEncounterManager), "Start")]
        class HunterAdder //Adds the extra hunters from the HunterCodes.txt for the random list when the game starts 
        {
            static void Postfix(ref List<PLEncounterManager.ShipLayout> ___PossibleHunters_LayoutData)
            {
                using (StreamReader streamReader = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "HunterCodes.txt"))) //This reads the file
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
                foreach (PLEncounterManager.ShipLayout ship in PLEncounterManager.Instance.PossibleHunters_LayoutData)
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
                            ship.CL += turret.GetDPS() * 0.1f * (1f - Mathf.Clamp01(turret.HeatGeneratedOnFire * 2.2f / turret.FireDelay));
                            ship.CL += turret.TurretRange * 0.0005f;
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
                            ship.ShipNameValue = biscuitnames[Random.Range(0, biscuitnames.Length - 1)];
                            break;
                    }
                    if(ship.GetCombatLevel() > 75 && Random.value < 0.1) 
                    {
                        ship.ShipNameValue = "The Glass Revenant Mk " + Random.Range(1, 999);
                    }
                }
            }
        }
    }
}
