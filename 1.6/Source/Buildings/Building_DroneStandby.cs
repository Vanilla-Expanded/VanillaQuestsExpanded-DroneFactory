using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_DroneStandby : Building
    {
        public Pawn drone;
        public override string Label => drone.Label + " (" + "VQE_Standby".Translate().ToLower() + ")";
        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            drone.DynamicDrawPhaseAt(DrawPhase.Draw, DrawPos);
        }

        public override AcceptanceReport ClaimableBy(Faction by)
        {
            return false;
        }

        public override AcceptanceReport DeconstructibleBy(Faction faction)
        {
            return false;
        }

        public override void TickRare()
        {
            base.TickRare();
            if (drone != null && drone.needs != null)
            {
                var lifespan = drone.needs.TryGetNeed<Need_Lifespan>();
                if (lifespan != null)
                {
                    lifespan.CurLevel -= (Need_Lifespan.BaseDrainPerDay / 60000f) * 250f * 0.1f;
                }
            }
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            yield return new Command_Action
            {
                defaultLabel = "VQE_Reactivate".Translate(),
                defaultDesc = "VQE_ReactivateDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Gizmos/Reactivate_Gizmo"),
                action = delegate
                {
                    Map.designationManager.AddDesignation(new Designation(this, InternalDefOf.VQE_ReactivateDrone_Designation));
                }
            };
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref drone, "drone");
        }
    }
}
