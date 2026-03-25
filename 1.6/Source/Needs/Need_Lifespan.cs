using Verse;
using RimWorld;
using System.Linq;
using System.Collections.Generic;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Need_Lifespan : Need
    {
        public const float BaseDrainPerDay = 0.016f;
        public override float MaxLevel
        {
            get
            {
                var multiplier = pawn.GetStatValue(InternalDefOf.VQE_LifespanMultiplier);
                return 1f * multiplier;
            }
        }

        public Need_Lifespan(Pawn pawn) : base(pawn)
        {
            threshPercents = [0.35f, 0.20f, 0.05f];
        }

        public override void SetInitialLevel() => CurLevel = MaxLevel;

        public override void NeedInterval()
        {
            if (IsFrozen) return;

            float drainMultiplier = 1f;
            if (pawn.MentalStateDef != null)
            {
                var ext = pawn.MentalStateDef.GetModExtension<DroneMentalStateExtension>();
                if (ext != null) drainMultiplier = ext.lifespanDrainMultiplier;
            }

            CurLevel -= (BaseDrainPerDay / 60000f) * drainMultiplier * 150f;

            if (CurLevel <= 0f && !pawn.Dead)
            {
                var core = pawn.health.hediffSet.GetNotMissingParts().FirstOrDefault(p => p.def == InternalDefOf.VQE_DroneCore);
                if (core != null) pawn.TakeDamage(new DamageInfo(DamageDefOf.Crush, 999f, 999f, -1f, null, core));
                else pawn.Kill(null);
            }
            else
            {
                CheckMalfunctions();
            }
        }

        private void CheckMalfunctions()
        {
            if (pawn.InMentalState || pawn.Dead || pawn.Downed) return;

            float pct = CurLevelPercentage;
            if (pct < 0.05f)
            {
                if (Rand.MTBEventOccurs(3f, 60000f, 150f)) TriggerMalfunction(MalfunctionTier.Extreme);
            }
            else if (pct < 0.20f)
            {
                if (Rand.MTBEventOccurs(9f, 60000f, 150f)) TriggerMalfunction(MalfunctionTier.Major);
            }
            else if (pct < 0.35f)
            {
                if (Rand.MTBEventOccurs(16f, 60000f, 150f)) TriggerMalfunction(MalfunctionTier.Minor);
            }
        }

        private void TriggerMalfunction(MalfunctionTier tier)
        {
            var candidates = DefDatabase<MentalStateDef>.AllDefs
                .Where(d => d.GetModExtension<DroneMentalStateExtension>()?.malfunctionTier == tier)
                .ToList();

            if (candidates.Count == 0) return;

            var selected = candidates.RandomElement();
            pawn.mindState.mentalStateHandler.TryStartMentalState(selected, null, true);
        }
    }
}
