using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(FloatMenuOptionProvider_DraftedMove), "PawnCanGoto")]
    public static class FloatMenuOptionProvider_DraftedMove_PawnCanGoto_Patch
    {
        public static void Postfix(Pawn pawn, IntVec3 gotoLoc, ref AcceptanceReport __result)
        {
            if (__result.Accepted && pawn.IsDrone())
            {
                if (!Utils.IsWithinTransmitter(gotoLoc, pawn.Map))
                {
                    __result = new AcceptanceReport("VQE_OutsideTransmitterRange".Translate());
                }
            }
        }
    }
}
