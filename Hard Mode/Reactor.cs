using HarmonyLib;
using UnityEngine;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLReactorInstance), "Update")]
    class Reactor_Radiation
    {
        static void Postfix(PLRadiationPoint ___RadPoint, PLReactorInstance __instance) //Increases radiation from reactor deppending on max temp and current stability
        {
            if (__instance.MyShipInfo.MyReactor != null && !PLGlobal.WithinTimeLimit(__instance.MyShipInfo.ReactorLastCoreEjectServerTime, PLServer.Instance.GetEstimatedServerMs(), 5000) && PhotonNetwork.isMasterClient) // Check to not cause a lot of exceptions with the reactor ejecting
            {
                PLReactor reactor = __instance.MyShipInfo.MyStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR, false);
                ___RadPoint.RaditationRange = reactor.TempMax / 150f;
                ___RadPoint.RaditationRange *= 1f + (__instance.MyShipInfo.CoreInstability * 4f);
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
                                plshipInfoBase.TakeDamage(dmg, false, EDamageType.E_ENERGY, Random.Range(0f, 1f), -1, ___MyOwner, -1);
                            }
                        }
                    }

                    __instance.VisibleMesh.SetActive(false);
                }
            }
        }

    }
}
