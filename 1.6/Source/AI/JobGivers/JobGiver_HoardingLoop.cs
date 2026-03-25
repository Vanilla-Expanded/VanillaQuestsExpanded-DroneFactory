using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobGiver_HoardingLoop : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            var item = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.ClosestTouch, TraverseParms.For(pawn), 9999f, t => !t.IsForbidden(pawn) && pawn.CanReserve(t));
            if (item != null)
            {
                RCellFinder.TryFindRandomCellNearWith(pawn.Position, c => Utils.IsWithinTransmitter(c, pawn.Map) && c.Walkable(pawn.Map), pawn.Map, out var dropCell, 5, 20);
                if (dropCell.IsValid)
                {
                    var job = JobMaker.MakeJob(JobDefOf.HaulToCell, item, dropCell);
                    job.count = item.stackCount;
                    job.ignoreDesignations = true;
                    return job;
                }
            }
            return null;
        }
    }
}
