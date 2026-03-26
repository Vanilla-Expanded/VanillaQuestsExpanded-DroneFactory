using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(ForbidUtility), "InAllowedArea")]
    public static class ForbidUtility_InAllowedArea_Patch
    {
        public static void Postfix(IntVec3 c, Pawn forPawn, ref bool __result)
        {
            Utils.ApplyDroneNetworkRestriction(ref __result, forPawn, c, forbidMode: false);
        }
    }
}
