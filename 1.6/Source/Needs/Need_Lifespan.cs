using Verse;
using RimWorld;
using UnityEngine;
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
                var years = pawn.GetStatValue(InternalDefOf.VQE_LifespanYears) * VanillaQuestsExpandedDroneFactory_Settings.lifespanMultiplier;
                return years;
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

            float drainMultiplier = pawn.GetStatValue(InternalDefOf.VQE_LifespanDrainSpeed);

            CurLevel -= (BaseDrainPerDay / 60000f) * drainMultiplier * 150f;

            if (CurLevel <= 0f && !pawn.Dead)
            {
                pawn.DestroyCore();
            }
            else
            {
                CheckMalfunctions();
            }
        }

        private void CheckMalfunctions()
        {
            if (pawn.InMentalState || pawn.Dead || pawn.Downed) return;

            if (pawn.def == InternalDefOf.VQE_CraftingDrone)
            {
                if (Rand.MTBEventOccurs(15f, 60000f, 150f) && pawn.mindState.mentalStateHandler.TryStartMentalState(InternalDefOf.VQE_Indexing, null, true))
                {
                    return;
                }
            }

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

        public void DrawBarOnGUI(Rect rect)
        {
            if (rect.height > 70f)
            {
                float num = (rect.height - 70f) / 2f;
                rect.height = 70f;
                rect.y += num;
            }
            if (Mouse.IsOver(rect))
            {
                TooltipHandler.TipRegion(rect, new TipSignal(() => GetTipString(), rect.GetHashCode()));
            }
            var rect3 = rect;
            float scale = 1f;
            if (def.scaleBar && MaxLevel < 1f)
            {
                scale = MaxLevel;
            }
            rect3.width *= scale;
            var barRect = Widgets.FillableBar(rect3, CurLevelPercentage);
            Widgets.FillableBarChangeArrows(rect3, GUIChangeArrow);
            if (threshPercents != null)
            {
                for (int i = 0; i < threshPercents.Count; i++)
                {
                    DrawBarThreshold(barRect, threshPercents[i] * scale);
                }
            }
            float curInstantLevelPercentage = CurInstantLevelPercentage;
            if (curInstantLevelPercentage >= 0f)
            {
                DrawBarInstantMarkerAt(rect3, curInstantLevelPercentage * scale);
            }
        }
    }
}
