using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(PawnRenderUtility), nameof(PawnRenderUtility.DrawEquipmentAndApparelExtras))]
    public static class PawnRenderUtility_DrawEquipmentAndApparelExtras_Patch
    {
        public static bool Prefix(Pawn pawn, Vector3 drawPos, Rot4 facing, PawnRenderFlags flags)
        {
            if (pawn.kindDef == InternalDefOf.VQE_TurretDrone)
            {
                var eq = pawn.equipment?.Primary;
                if (eq != null)
                {
                    var stance = pawn.stances?.curStance as Stance_Busy;
                    float aimAngle = facing.AsAngle;
                    
                    if (!flags.HasFlag(PawnRenderFlags.NeverAimWeapon) && stance != null && !stance.neverAimWeapon && stance.focusTarg.IsValid)
                    {
                        Vector3 targetPos = stance.focusTarg.HasThing ? stance.focusTarg.Thing.DrawPos : stance.focusTarg.Cell.ToVector3Shifted();
                        if ((targetPos - pawn.DrawPos).MagnitudeHorizontalSquared() > 0.001f)
                        {
                            aimAngle = (targetPos - pawn.DrawPos).AngleFlat();
                        }
                        Verb verb = pawn.CurrentEffectiveVerb;
                        if (verb != null && verb.AimAngleOverride.HasValue)
                        {
                            aimAngle = verb.AimAngleOverride.Value;
                        }
                    }
                    
                    Vector3 eqDrawPos = pawn.DrawPos;
                    eqDrawPos.y += 0.1f;
                    
                    PawnRenderUtility.DrawEquipmentAiming(eq, eqDrawPos, aimAngle);
                }
                return false;
            }
            return true;
        }
    }
}
