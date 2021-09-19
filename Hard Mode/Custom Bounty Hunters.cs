using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using System.Reflection.Emit;
using PulsarModLoader.Patches;
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
                using (StreamReader streamReader = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "HunterCodes.txt")))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string bountyhunter = streamReader.ReadLine();
                        if (bountyhunter.Length > 50)
                        {
                            ___PossibleHunters_LayoutData.Add(new PLEncounterManager.ShipLayout(bountyhunter));
                        }
                    }
                }
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
        [HarmonyPatch(typeof(PLServer), "SpawnHunter")]
        public class BountyHunterBalance //Allow to change the Min and Max values for the difference between you and the bounty hunter
        {
            static public float MinCombatLevel = 1.2f;
            static public float MaxCombatLevel = 1.5f;
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
            static void Posftix(ref PLShipInfoBase __result)
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
                            ship.ShipNameValue = biscuitnames[UnityEngine.Random.Range(0, biscuitnames.Length - 1)];
                            break;
                    }
                    if((ship.IsRelicHunter || ship.IsBountyHunter) && UnityEngine.Random.value < 0.1 ) 
                    {
                        ship.ShipNameValue = "The Glass Revenant Mk" + UnityEngine.Random.Range(1, 999);
                    }
                }
            }
        }
    }
}
