using Verse;
using Verse.AI;
using System.Collections.Generic;
using RimWorld;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobDriver_ReactivateDrone : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed) => pawn.Reserve(TargetA, job, 1, -1, null, errorOnFailed);
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.WaitWith(TargetIndex.A, 60);
            yield return Toils_General.Do(delegate
            {
                var building = TargetA.Thing as Building_DroneStandby;
                if (building != null)
                {
                    GenSpawn.Spawn(building.drone, building.Position, building.Map);
                    Find.Selector.Select(building.drone);
                    building.Destroy();
                    MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
                }
            });
        }
    }
}
