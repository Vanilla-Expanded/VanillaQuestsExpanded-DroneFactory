using Verse;
using Verse.AI;
using System.Collections.Generic;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobDriver_EnterStandby : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed) => true;
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_General.WaitWith(TargetIndex.A, 120, useProgressBar: true);
            yield return Toils_General.Do(delegate
            {
                var building = ThingMaker.MakeThing(InternalDefOf.VQE_DroneStandby) as Building_DroneStandby;
                building.drone = pawn;
                GenSpawn.Spawn(building, pawn.Position, pawn.Map);
                Find.Selector.Select(building);
                pawn.DeSpawn();
            });
        }
    }
}
