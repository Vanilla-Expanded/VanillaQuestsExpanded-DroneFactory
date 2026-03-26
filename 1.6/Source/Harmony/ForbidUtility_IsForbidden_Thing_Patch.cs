using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(ForbidUtility), "IsForbidden", [typeof(Thing), typeof(Pawn)])]
    public static class ForbidUtility_IsForbidden_Thing_Patch
    {
        public static void Postfix(Thing t, Pawn pawn, ref bool __result)
        {
            if (!t.Spawned) return;
            Utils.ApplyDroneNetworkRestriction(ref __result, pawn, t.Position, forbidMode: true);
        }
    }
}
