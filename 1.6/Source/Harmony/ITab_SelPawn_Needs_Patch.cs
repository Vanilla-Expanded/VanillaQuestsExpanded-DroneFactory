using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(ITab), "SelPawn", MethodType.Getter)]
    public static class ITab_SelPawn_Needs_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(ITab __instance, ref Pawn __result)
        {
            if (__instance is ITab_Pawn_Needs && Find.Selector.SingleSelectedThing is Building_DroneStandby b)
            {
                __result = b.drone;
                return false;
            }
            return true;
        }
    }
}
