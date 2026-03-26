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
            Utils.ApplyDroneNetworkRestriction(ref __result, pawn, c, forbidMode: true);
        }
    }
}
