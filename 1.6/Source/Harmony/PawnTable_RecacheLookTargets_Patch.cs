using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(PawnTable), "RecacheLookTargets")]
    public static class PawnTable_RecacheLookTargets_Patch
    {
        public static void Postfix(PawnTable __instance, ref List<LookTargets> ___cachedLookTargets, List<Pawn> ___cachedPawns)
        {
            if (__instance is PawnTable_Drones)
            {
                for (int i = 0; i < ___cachedPawns.Count; i++)
                {
                    var pawn = ___cachedPawns[i];
                    if (!pawn.Spawned)
                    {
                        var buildings = Find.CurrentMap?.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQE_DroneStandby);
                        var building = buildings?.FirstOrDefault(b => b is Building_DroneStandby droneStandby && droneStandby.drone == pawn) as Building_DroneStandby;
                        if (building != null)
                        {
                            ___cachedLookTargets[i] = new LookTargets(building);
                        }
                    }
                }
            }
        }
    }
}
