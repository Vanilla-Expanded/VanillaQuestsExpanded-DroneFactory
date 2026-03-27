using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory

{
    public class CompProximityFuseWithCount : ThingComp
    {
        public CompProperties_ProximityFuseWithCount Props => (CompProperties_ProximityFuseWithCount)props;

        public override void CompTick()
        {
            if (Find.TickManager.TicksGame % 250 == 0)
            {
                CompTickRare();
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Trigger",
                    action = delegate
                    {
                        parent.GetComp<CompExplosive>().StartWick();
                    }
                };
            }
        }

        public override void CompTickRare()
        {
            Pawn pawn = parent as Pawn;
            if (parent.Spawned && !pawn.Dead)
            {
                int count = 0;
                foreach (var cell in GenRadial.RadialCellsAround(parent.Position, Props.radius, true))
                {
                    if (!cell.InBounds(parent.Map)) continue;

                    List<Thing> things = parent.Map.thingGrid.ThingsListAtFast(cell);
                    for (int i = 0; i < things.Count; i++)
                    {
                        if (things[i].def == Props.target)
                        {
                            count++;
                            break;
                        }
                    }
                }

                if (count >= Props.amount || pawn.IsBurning())
                {
                    parent.GetComp<CompExplosive>().StartWick();

                }
            }
        }
    }
}