using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(JobGiver_ReactToCloseMeleeThreat), "TryGiveJob")]
    public static class JobGiver_ReactToCloseMeleeThreat_TryGiveJob_Patch
    {
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.mindState.meleeThreat != null && pawn.IsDrone() && pawn.Faction == pawn.mindState.meleeThreat.Faction)
            {
                return false;
            }
            return true;
        }
    }
}
