using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobGiver_Hardcrash : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            var building = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.Touch, TraverseParms.For(pawn), 9999f, t => pawn.CanReserve(t));
            if (building != null) return JobMaker.MakeJob(JobDefOf.AttackMelee, building);
            return null;
        }
    }
}
