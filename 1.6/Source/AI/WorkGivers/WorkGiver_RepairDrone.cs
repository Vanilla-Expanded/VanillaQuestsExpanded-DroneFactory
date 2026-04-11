using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;

namespace VanillaQuestsExpandedDroneFactory
{
    public class WorkGiver_RepairDrone : WorkGiver_Drone
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.PawnsInFaction(pawn.Faction).Where(p => p.IsDrone());
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t is Pawn drone && drone.IsDrone())
            {
                if (drone.health.hediffSet.GetMissingPartsCommonAncestors().Any() || drone.health.hediffSet.hediffs.Any(h => h is Hediff_Injury))
                {
                    var comp = drone.GetComp<CompDrone>();
                    if (!forced && !comp.autoRepair) return null;
                    if (!CanWorkOnThing(pawn, t, forced)) return null;
                    return JobMaker.MakeJob(InternalDefOf.VQE_RepairDrone, t);
                }
            }
            return null;
        }
    }
}
