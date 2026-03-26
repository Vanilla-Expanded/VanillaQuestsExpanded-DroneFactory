using Verse;
using RimWorld;
using Verse.AI;
using UnityEngine;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobGiver_HoardingLoop : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.MentalState is MentalState_HoardingLoop hoardingState)
            {
                var item = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.ClosestTouch, TraverseParms.For(pawn), 9999f, t => !t.IsForbidden(pawn) && pawn.CanReserve(t) && !hoardingState.hoardedItemIds.Contains(t.thingIDNumber));
                if (item != null)
                {
                    RCellFinder.TryFindRandomCellNearWith(pawn.Position, c => Utils.IsWithinTransmitter(c, pawn.Map) && c.Walkable(pawn.Map) && GenSpawn.CanSpawnAt(item.def, c, pawn.Map), pawn.Map, out var dropCell, 5, 20);
                    if (dropCell.IsValid)
                    {
                        hoardingState.hoardedItemIds.Add(item.thingIDNumber);
                        var job = JobMaker.MakeJob(JobDefOf.HaulToCell, item, dropCell);
                        job.count = Mathf.Min(item.stackCount, (int)(pawn.GetStatValue(StatDefOf.CarryingCapacity) / item.def.VolumePerUnit));
                        job.ignoreDesignations = true;
                        return job;
                    }
                }
                else
                {
                    hoardingState.hoardedItemIds.Clear();
                }
            }
            return null;
        }
    }
}
