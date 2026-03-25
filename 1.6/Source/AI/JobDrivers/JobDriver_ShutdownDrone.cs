using Verse;
using RimWorld;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;

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
                var core = drone.health.hediffSet.GetNotMissingParts().FirstOrDefault(p => p.def == InternalDefOf.VQE_DroneCore);
                if (core is null) return;
                var damage = new DamageInfo(DamageDefOf.Crush, 999f, 999f, -1f, null, core);
                damage.SetAllowDamagePropagation(false);
                drone.TakeDamage(damage);
                if (drone.Dead is false)
                    drone.Kill(null);
            });
        }
    }
}
