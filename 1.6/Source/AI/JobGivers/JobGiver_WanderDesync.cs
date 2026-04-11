using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobGiver_WanderDesync : JobGiver_Wander
    {
        protected override IntVec3 GetExactWanderDest(Pawn pawn)
        {
            RCellFinder.TryFindRandomCellNearWith(pawn.Position, c => pawn.RequiresTransmitter() && !Utils.IsWithinTransmitter(c, pawn.Map), pawn.Map, out var result, 10, 30);
            return result;
        }
        protected override IntVec3 GetWanderRoot(Pawn pawn) => pawn.Position;
    }
}
