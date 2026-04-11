using Verse;
using Verse.AI;
using System.Collections.Generic;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobDriver_ShutdownDrone : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed) => pawn.Reserve(TargetA, job, 1, -1, null, errorOnFailed);
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.WaitWith(TargetIndex.A, 100, useProgressBar: true);
            yield return Toils_General.Do(delegate
            {
                var drone = (Pawn)TargetA.Thing;
                drone.DestroyCore();
            });
        }
    }
}
