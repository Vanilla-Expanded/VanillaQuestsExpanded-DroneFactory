using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobGiver_StandIfOutOfTransmitterRange : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (!Utils.IsWithinTransmitter(pawn.Position, pawn.Map))
            {
                var job = JobMaker.MakeJob(InternalDefOf.VQE_StandOutOfTransmitterRange);
                job.expiryInterval = 60;
                return job;
            }
            return null;
        }
    }
}
