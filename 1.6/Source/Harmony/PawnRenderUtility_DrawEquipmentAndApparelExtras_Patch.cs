using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(PawnRenderUtility), nameof(PawnRenderUtility.DrawEquipmentAndApparelExtras))]
    public static class PawnRenderUtility_DrawEquipmentAndApparelExtras_Patch
    {
        public static void Prefix(Pawn pawn, ref Vector3 drawPos, Rot4 facing)
        {
            if (pawn.kindDef == InternalDefOf.VQE_TurretDrone && facing == Rot4.North)
            {
                drawPos.y += 0.1f;
            }
        }
    }
}
