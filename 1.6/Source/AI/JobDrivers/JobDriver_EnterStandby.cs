using Verse;
using Verse.AI;
using System.Collections.Generic;
using RimWorld;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobDriver_EnterStandby : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed) => true;
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_General.Wait(120).WithProgressBarToilDelay(TargetIndex.None);
            yield return Toils_General.Do(delegate
            {
                var building = ThingMaker.MakeThing(InternalDefOf.VQE_DroneStandby) as Building_DroneStandby;
                building.drone = pawn;
                building.SetFaction(pawn.Faction);
                GenSpawn.Spawn(building, pawn.Position, pawn.Map);
                Find.Selector.Select(building);
                pawn.DeSpawn();
                MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
            });
        }
    }
}
