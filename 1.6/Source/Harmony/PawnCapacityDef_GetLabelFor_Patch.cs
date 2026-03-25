using System;
using HarmonyLib;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(PawnCapacityDef), "GetLabelFor", [typeof(Pawn)])]
    public static class PawnCapacityDef_GetLabelFor_Patch
    {
        public static void Postfix(Pawn pawn, ref string __result, PawnCapacityDef __instance)
        {
            if (StaticCollections.pawnCapacityLabels.ContainsKey(pawn.def) && StaticCollections.pawnCapacityLabels[pawn.def].ContainsKey(__instance))
            {
                __result = StaticCollections.pawnCapacityLabels[pawn.def][__instance];
            }
        }
    }
}
