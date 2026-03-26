using System.Collections.Generic;
using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(Pawn_DraftController), "GetGizmos")]
    public static class Pawn_DraftController_GetGizmos_Patch
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn_DraftController __instance)
        {
            foreach (var v in __result)
            {
                if (__instance.pawn.IsDrone())
                {
                    var report = Utils.CanDraftDrone(__instance.pawn);
                    if (!report.Accepted && !report.Reason.NullOrEmpty())
                    {
                        v.Disable(report.Reason);
                    }
                }
                yield return v;
            }
        }
    }
}
