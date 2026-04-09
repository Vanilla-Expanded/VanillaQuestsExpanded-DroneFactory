using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobGiver_IdleFacingTarget : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.MentalState is not MentalState_Indexing mentalState) return null;
            if (mentalState.target != null && mentalState.target.Spawned)
            {
                pawn.rotationTracker.FaceTarget(mentalState.target);
            }
            return JobMaker.MakeJob(JobDefOf.Wait, 60);
        }
    }
}
