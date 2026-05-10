using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_AutomatedDroneAssembler : Building
    {

        private Effecter effecter;
        public WeightedDrones[] array = new WeightedDrones[3];
        public PawnKindDef currentPawn;
        public int currentTimer = -1;
        public int tickCounter = -1;

        public struct WeightedDrones
        {
            public PawnKindDef pawn;
            public float weight;
            public int ticks;
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (effecter is null)
            {
                effecter = EffecterDefOf.ConstructMetal.SpawnAttached(this, Map);
            }

            WeightedDrones drone = new WeightedDrones
            {
                pawn = InternalDefOf.VQE_BattleDrone,
                weight = 5f,
                ticks = 2700
            };
            array[0] = drone;
            drone = new WeightedDrones
            {
                pawn = InternalDefOf.VQE_RaiderDrone,
                weight = 1.6f,
                ticks = 4800
            };
            array[1] = drone;
            drone = new WeightedDrones
            {
                pawn = InternalDefOf.VQE_HornetDrone,
                weight = 2f,
                ticks = 4450
            };
            array[2] = drone;
            if (!respawningAfterLoad)
            {
                WeightedDrones droneChosen = array.RandomElementByWeight(x => x.weight);
                currentPawn = droneChosen.pawn;
                currentTimer = droneChosen.ticks;
            }
        }

        protected override void TickInterval(int delta)
        {
            base.TickInterval(delta);
            if (effecter != null)
            {
                effecter.EffectTick(this, this);
            }

            if (tickCounter > currentTimer)
            {
                tickCounter = 0;
                Pawn p = PawnGenerator.GeneratePawn(currentPawn, Faction.OfAncientsHostile);
                GenSpawn.Spawn(p, this.InteractionCell, this.Map);
                WeightedDrones droneChosen = array.RandomElementByWeight(x => x.weight);
                currentPawn = droneChosen.pawn;
                currentTimer = droneChosen.ticks;
            }

            tickCounter = tickCounter + delta;
        }

        /*public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
        {
            base.DynamicDrawPhaseAt(phase, drawLoc, flip);

            var vector = this.DrawPos + Altitudes.AltIncVect;
            vector.y += 5;
            GraphicsCache.graphicUnfinishedDrone.DrawFromDef(vector, Rot4.North, null);
        }*/

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);
            var vector = this.DrawPos + Altitudes.AltIncVect;
            vector.y += 5;
            GraphicsCache.graphicUnfinishedDrone.DrawFromDef(vector, Rot4.North, null);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref currentPawn, "currentPawn");
            Scribe_Values.Look(ref currentTimer, "currentTimer");
            Scribe_Values.Look(ref tickCounter, "tickCounter");

        }

        public override string GetInspectString()
        {
            var sb = new StringBuilder(base.GetInspectString());
            if (currentPawn != null && currentTimer != -1)
            {
                sb.AppendLine("VQE_DroneSpawningIn".Translate(currentPawn.label.CapitalizeFirst(), (currentTimer - tickCounter).ToStringTicksToPeriod()));

            }

            return sb.ToString().TrimEndNewlines();
        }
    }
}
