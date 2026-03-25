using HarmonyLib;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(PawnCapacityDef), "CanShowOnPawn")]
    public static class PawnCapacityDef_CanShowOnPawn_Patch
    {
        public static void Postfix(Pawn p, ref bool __result, PawnCapacityDef __instance)
        {
            if (p.IsDrone())
            {
                __result = __instance.showOnDrones;
            }
        }
    }
}
