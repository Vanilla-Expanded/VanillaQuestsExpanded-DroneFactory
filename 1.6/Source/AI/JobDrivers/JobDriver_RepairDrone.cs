using Verse;
using RimWorld;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobDriver_RepairDrone : JobDriver
    {
        private Pawn Drone => (Pawn)job.GetTarget(TargetIndex.A).Thing;

        private int ticksToNextRepair;

        private int TicksPerHeal => Mathf.RoundToInt(1f / pawn.GetStatValue(StatDefOf.GeneralLaborSpeed) * 120f);

        public override bool TryMakePreToilReservations(bool errorOnFailed) => pawn.Reserve(Drone, job, 1, -1, null, errorOnFailed);

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);

            var repair = Toils_General.WaitWith(TargetIndex.A, int.MaxValue, false, true, true);
            repair.WithEffect(EffecterDefOf.ConstructMetal, TargetIndex.A);
            repair.AddPreInitAction(delegate
            {
                ticksToNextRepair = TicksPerHeal;
            });
            repair.handlingFacing = true;
            repair.tickIntervalAction = delegate(int delta)
            {
                ticksToNextRepair -= delta;
                if (ticksToNextRepair <= 0)
                {
                    ticksToNextRepair = TicksPerHeal;
                    var injuries = new List<Hediff_Injury>();
                    Drone.health.hediffSet.GetHediffs(ref injuries);
                    var injury = injuries.Where(i => !i.IsPermanent()).RandomElementWithFallback();
                    if (injury != null)
                    {
                        float healAmount = Mathf.Min(injury.Severity, 1f);
                        injury.Heal(healAmount);
                        var lifespan = Drone.needs.TryGetNeed<Need_Lifespan>();
                        if (lifespan != null) lifespan.CurLevel -= 0.001f * healAmount;
                        if (injury.Severity <= 0f) Drone.health.RemoveHediff(injury);
                    }
                    else
                    {
                        var missingPart = Drone.health.hediffSet.GetMissingPartsCommonAncestors().RandomElementWithFallback();
                        if (missingPart != null)
                        {
                            Drone.health.RestorePart(missingPart.Part);
                            var lifespan = Drone.needs.TryGetNeed<Need_Lifespan>();
                            if (lifespan != null) lifespan.CurLevel -= missingPart.Part.def.GetMaxHealth(Drone) * 0.001f;
                        }
                    }
                }
                pawn.rotationTracker.FaceTarget(Drone);
                if (pawn.skills != null)
                {
                    pawn.skills.Learn(SkillDefOf.Construction, 0.05f * (float)delta);
                }
            };
            repair.AddEndCondition(() => CanRepair() ? JobCondition.Ongoing : JobCondition.Succeeded);
            repair.activeSkill = () => SkillDefOf.Construction;
            yield return repair;
        }

        private bool CanRepair()
        {
            var injuries = new List<Hediff_Injury>();
            Drone.health.hediffSet.GetHediffs(ref injuries);
            if (injuries.Any(i => !i.IsPermanent())) return true;
            if (Drone.health.hediffSet.GetMissingPartsCommonAncestors().Any()) return true;
            return false;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksToNextRepair, "ticksToNextRepair", 0);
        }
    }
}
