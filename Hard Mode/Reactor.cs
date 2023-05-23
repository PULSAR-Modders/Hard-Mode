using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLReactorInstance), "Update")]
    class Reactor_Radiation
    {

        static void Postfix(PLRadiationPoint ___RadPoint, PLReactorInstance __instance) //Increases radiation from reactor deppending on max temp and current stability
        {
            PLTempRadius tempRadius = null;
            if (__instance.MyShipInfo != null && __instance.MyShipInfo.MyReactor != null && __instance.MyShipInfo.MyStats != null && ___RadPoint != null)//Ensure no Null values will be used
            {
                if (!PLGlobal.WithinTimeLimit(__instance.MyShipInfo.ReactorLastCoreEjectServerTime, PLServer.Instance.GetEstimatedServerMs(), 5000) && Options.DangerousReactor) // Check to not cause a lot of exceptions with the reactor ejecting
                {
                    PLReactor reactor = __instance.MyShipInfo.MyStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR, false);
                    if (reactor != null)
                    {
                        ___RadPoint.RaditationRange = reactor.TempMax / 500f * (1f + (__instance.MyShipInfo.CoreInstability * 4f));
                        ___RadPoint.RadScale = __instance.MyShipInfo.CoreInstability / 3f;
                        if (___RadPoint.RaditationRange < 25f) ___RadPoint.RaditationRange = 25; // This is so sylvassi reactor doesn't become radiation proof

                        if (__instance.gameObject.GetComponent<PLTempRadius>() != null) //This part adds temperature range to reactor, so it changes the ship interior temperature
                        {
                            tempRadius = __instance.gameObject.GetComponent<PLTempRadius>();
                        }
                        else
                        {
                            tempRadius = __instance.gameObject.AddComponent<PLTempRadius>();
                            tempRadius.IsOnShip = true;
                        }
                        tempRadius.MinRange = 10f + __instance.MyShipInfo.MyStats.ReactorTempCurrent / (__instance.MyShipInfo.MyStats.ReactorTempMax * 0.5f);
                        tempRadius.MaxRange = 20f + __instance.MyShipInfo.MyStats.ReactorTempCurrent / (__instance.MyShipInfo.MyStats.ReactorTempMax * 0.25f);
                        tempRadius.Temperature = __instance.MyShipInfo.MyStats.ReactorTempCurrent / (__instance.MyShipInfo.MyStats.ReactorTempMax * 0.3f);
                        if (tempRadius.Temperature < 1) tempRadius.Temperature = 1; //This is so reactor doesn't decide to make the nearby area colder
                        else if (tempRadius.Temperature > 20) tempRadius.Temperature = 10; //This is more for the OP hunters that have reactor that would just kill them with the temp
                        if (__instance.MyShipInfo.MyStats.ReactorTempCurrent >= __instance.MyShipInfo.MyStats.ReactorTempMax * 0.90 && Random.Range(0, 600) == 14) //this spawns fire when too hot
                        {
                            PLMainSystem system = __instance.MyShipInfo.GetSystemFromID(Random.Range(0, 4));
                            int looplimit = 0;
                            while ((system == null || system.IsOnFire()) && looplimit < 20)
                            {
                                system = __instance.MyShipInfo.GetSystemFromID(Random.Range(0, 4));
                                looplimit++;
                            }
                            if (system != null) PLServer.Instance.CreateFireAtSystem(system, false);
                        }
                    }
                }
                else if (!Options.DangerousReactor && __instance.gameObject.GetComponent<PLTempRadius>() != null) //This will bring things to normal when dangerous reactor is disabled
                {
                    Object.Destroy(__instance.gameObject.GetComponent<PLTempRadius>());
                    ___RadPoint.RaditationRange = 1f;
                }
            }
        }
    }
    /*
    [HarmonyPatch(typeof(PLReactor), "ShipUpdate")]
    class ShieldUsage
    {
        static void Postfix(PLShipInfoBase inShipInfo)
        {
            PLShieldGenerator shield = inShipInfo.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD, false);
            shield.IsPowerActive = true;
            shield.RequestPowerUsage_Limit = shield.CalculatedMaxPowerUsage_Watts * 0.25f;
            shield.InputPower_Watts = shield.CalculatedMaxPowerUsage_Watts * 0.25f;
            shield.RequestPowerUsage_Percent = 1f;

        }
    }
    */
    [HarmonyPatch(typeof(PLEnergySphere), "Detonate")]
    class Explosion
    {
        static void Prefix(PLEnergySphere __instance, bool ___HasExploded, PLShipInfoBase ___MyOwner, float ___MaxRange) // This should make the damage from the reactor more powerfull deppending on the price
        {
            if (!PhotonNetwork.isMasterClient)
            {
                return;
            }
            if (!___HasExploded)
            {
                ___HasExploded = true;
                if (___MyOwner != null)
                {
                    PLEnergySphereExplosion component = UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.EnergySphereExplosionPrefab, __instance.transform.position, __instance.transform.rotation).GetComponent<PLEnergySphereExplosion>();
                    if (component != null)
                    {
                        component.Init(__instance.DamageMultiplier - 8f, ___MaxRange);
                    }
                    PLNetworkManager.Instance.WaitForNetworkDestroy(__instance.gameObject, 2f);
                    foreach (PLShipInfoBase plshipInfoBase in PLEncounterManager.Instance.AllShips.Values)
                    {
                        if (plshipInfoBase != null)
                        {
                            float magnitude = (plshipInfoBase.Exterior.transform.position - __instance.transform.position).magnitude;
                            if (magnitude < ___MaxRange)
                            {
                                float num = Mathf.Clamp01(magnitude / ___MaxRange);
                                num += 0.001f;
                                float dmg = Mathf.Clamp01(1f - num) * 1800f;
                                plshipInfoBase.TakeDamage(dmg, false, EDamageType.E_ENERGY, UnityEngine.Random.Range(0f, 1f), -1, ___MyOwner, -1);
                            }
                        }
                    }

                    __instance.VisibleMesh.SetActive(false);
                }
            }
            foreach (PLTempRadius temp in PLGameStatic.Instance.m_TempRadius) //This makes the reactor exploding without ejecting also cool down the ship
            {
                if (temp.IsOnShip == true) temp.Temperature = 1;
            }
        }

    }

    [HarmonyPatch(typeof(PLServer), "ServerEjectReactorCore")]
    class ResetTemperature //This makes that when reactor is ejected the temperature go back to normal levels
    {
        static void Postfix()
        {
            foreach (PLTempRadius temp in PLGameStatic.Instance.m_TempRadius)
            {
                if (temp.IsOnShip == true) temp.Temperature = 1;
            }
        }
    }

    [HarmonyPatch(typeof(PLReactor), "Tick")]
    class ReactorsNerf
    {
        private static FieldInfo OriginalEnergyOutputMax = AccessTools.Field(typeof(PLReactor), "OriginalEnergyOutputMax");
        static void Postfix(PLReactor __instance)
        {
            if (Options.MasterHasMod)
            {
                if (__instance.SubType >= PulsarModLoader.Content.Components.Reactor.ReactorModManager.Instance.VanillaReactorMaxType) return;
                float multiplier = 0.5f;
                EReactorType type = (EReactorType)__instance.SubType;
                float _OriginalEnergyOutputMax = (float)OriginalEnergyOutputMax.GetValue(__instance);
                if (type != EReactorType.E_REAC_STRONGPOINT && type != EReactorType.THERMOCORE_REACTOR)__instance.EnergyOutputMax = Options.WeakReactor ? _OriginalEnergyOutputMax * multiplier : _OriginalEnergyOutputMax;
                if(type == EReactorType.ANCIENT_REACTOR) 
                {
                    __instance.EnergyOutputMax = 150000f;
                    OriginalEnergyOutputMax.SetValue(__instance, __instance.EnergyOutputMax );
                }
                if (type == EReactorType.THERMOCORE_REACTOR)
                {
                    OriginalEnergyOutputMax.SetValue(__instance, Options.WeakReactor ? 38000f * multiplier : 38000f );
                }
                if (type == EReactorType.E_REAC_STRONGPOINT)
                {
                    OriginalEnergyOutputMax.SetValue(__instance, Options.WeakReactor ? 24000f * multiplier : 24000f );
                }
                /*switch (type) 
                {
                    default:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 15000f* multiplier : 15000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.E_REAC_WD_NULL_POINT_REACTOR_B:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 18000f * multiplier : 18000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.E_REAC_CU_FUSION_REACTOR:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 14500f * multiplier : 14500f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.E_REAC_CU_FUSION_REACTOR_MK2:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 17000f * multiplier : 17000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.E_REAC_CU_FUSION_REACTOR_MK3:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 22000f * multiplier : 22000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.E_REAC_FB_MINI_REACTOR:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 12600f * multiplier : 12600f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.E_REAC_GTC_QUIET_CUPCAKE:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 14800f * multiplier : 14800f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.E_REAC_PF_ANTIMATTER_REACTOR:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 25000f * multiplier : 25000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.ANCIENT_REACTOR:
                        __instance.EnergyOutputMax = 150000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.ROLAND_REACTOR:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 23000f * multiplier : 23000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.THERMOCORE_REACTOR:
                        __instance.OriginalEnergyOutputMax = Options.WeakReactor ? 38000f * multiplier : 38000f;
                        break;
                    case EReactorType.E_REAC_STRONGPOINT:
                        __instance.OriginalEnergyOutputMax = Options.WeakReactor ? 24000f * multiplier : 24000f;
                        break;
                    case EReactorType.E_LEAKING_REACTOR:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 21000f * multiplier : 21000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.E_SYLVASSI_REACTOR:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 20000f * multiplier : 20000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                    case EReactorType.E_POLYTECH_ORIGINAL:
                        __instance.EnergyOutputMax = Options.WeakReactor ? 30000f * multiplier : 30000f;
                        __instance.OriginalEnergyOutputMax = __instance.EnergyOutputMax;
                        break;
                }
                */
            }
        }
        /*
        public static float NullPoint = 15000f;
        public static float ReinforcedNullPoint = 18000f;
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions)
        {
            List<CodeInstruction> instructionsList = Instructions.ToList();
            instructionsList[15].operand = 0xC350;
            instructionsList[19].operand = 10000f;
            instructionsList[25].operand = 26f;
            return instructionsList.AsEnumerable();
        }
        */
    }
}
