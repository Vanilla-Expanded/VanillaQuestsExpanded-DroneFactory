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
            if (pawn.IsDrone() && !__instance.labelDrones.NullOrEmpty())
            {
                __result = __instance.labelDrones;
            }
        }
    }
}
