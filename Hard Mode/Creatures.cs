using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using PulsarPluginLoader.Patches;
namespace Hard_Mode
{
    class Creatures //All creatures will be here to help with ballancing and changing stats
    {
        [HarmonyPatch(typeof(PLAirElemental), "Start")]
        class Tornados
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLAnt), "Start")]
        class Ant
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLAntArmored), "Start")]
        class ArmoredAnt
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLAntHeavy), "Start")]
        class HeavyAnt
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLAntRavager), "Start")]
        class RavangerAnt
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLBanditLandDrone), "Start")]
        class BanditLandDrone
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLBrainCreature), "Start")]
        class Brain
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider), "Start")]
        class InfectedCrawlers
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider_Medium), "Start")]
        class MediumInfectedSpider
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider_WG), "Start")]
        class GuardianInfectedSpider
        {
            static void Postfix(PLInfectedSpider_WG __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.Health = __instance.MaxHealth;
                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider_WG), "Update")]
        class GuadianInfectedUpdate
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions)
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[6].operand = 0x7530;       
                return instructionsList.AsEnumerable();
            }
            
        }

        [HarmonyPatch(typeof(PLInfectedSwarm), "Start")]
        class Dontknowwhatthisis
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLInfectedBoss), "Start")]
        class AlsoDontKnow
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLLCLabEnemy), "Start")]
        class ColonyGhost
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLRaptor), "Start")]
        class Raptor
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLRobotWalker), "Start")]
        class Paladin
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLRobotWalkerLarge), "Start")]
        class ElitPaladin
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLSpider), "Start")]
        class Spider
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLRat), "Start")]
        class Rat
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLSlime), "Start")]
        class Slime
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLWasteWasp), "Start")]
        class Wasp
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLSlimeBoss), "Start")]
        class WastedWingSlime
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLStalkerPawn), "Start")]
        class Stalker
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedScientist), "Start")]
        class WastedWingScientists
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLCrystalBoss), "Start")]
        class TheSource
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedBoss_WDFlagship), "Start")]
        class MindSlaver
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLInfectedHeart_WDFlagship), "Start")]
        class ForsakenFlagshipHeart
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLInfectedCrewmember), "Start")]
        class InfectedScientits
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLAssassinBot), "Start")]
        class AssassinBot
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLBoardingBot), "Start")]
        class BoardingBot
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLGiantRobotHead), "Start")]
        class DownedProtector
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLGroundTurret), "Start")]
        class GroundTurret
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLRoamingSecurityGuardRobot), "Start")]
        class MadmansMansionDrone
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }

            }
        }

        [HarmonyPatch(typeof(PLSmokeCreature), "Start")]
        class CyphersSmoke
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
    }
}
