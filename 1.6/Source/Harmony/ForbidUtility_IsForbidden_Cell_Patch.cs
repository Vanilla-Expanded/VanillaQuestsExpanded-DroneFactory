using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(ForbidUtility), "IsForbidden", [typeof(IntVec3), typeof(Pawn)])]
    public static class ForbidUtility_IsForbidden_Cell_Patch
    {
        public static void Postfix(IntVec3 c, Pawn pawn, ref bool __result)
        {
            if (__result || pawn == null || pawn.Map == null) return;
            if (pawn.IsDrone())
            {
                bool reverse = false;
                if (pawn.MentalStateDef != null)
                {
                    var ext = pawn.MentalStateDef.GetModExtension<DroneMentalStateExtension>();
                    reverse = ext?.reverseNetworkRestriction ?? false;
                }
                var withinRange = Utils.IsWithinTransmitter(c, pawn.Map);
                if (reverse)
                {
                    if (withinRange) __result = true;
                }
                else
                {
                    if (!withinRange) __result = true;
                }
            }
        }
    }
}
