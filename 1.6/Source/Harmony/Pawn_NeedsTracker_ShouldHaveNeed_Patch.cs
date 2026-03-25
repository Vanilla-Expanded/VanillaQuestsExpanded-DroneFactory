using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
    public static class Pawn_NeedsTracker_ShouldHaveNeed_Patch
    {
        public static void Postfix(Pawn ___pawn, NeedDef nd, ref bool __result)
        {
            if (nd == InternalDefOf.VQE_Lifespan)
            {
                var isDrone = ___pawn.IsDrone();
                if (__result is false && isDrone)
                {
                    __result = true;
                }
                else if (__result && isDrone is false)
                {
                    __result = false;
                }
            }
        }
    }
}
