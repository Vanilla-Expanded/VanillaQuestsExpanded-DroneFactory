using Verse;
using RimWorld;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;

namespace VanillaQuestsExpandedDroneFactory
{
    public class WorkGiver_RepairDrone : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.Touch;
        public override Danger MaxPathDanger(Pawn pawn) => Danger.Deadly;

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.PawnsInFaction(pawn.Faction).Where(p => p.IsDrone());
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t is Pawn drone && drone.IsDrone())
            {
                if (drone.health.hediffSet.GetMissingPartsCommonAncestors().Any() || drone.health.hediffSet.hediffs.Any(h => h is Hediff_Injury))
                {
                    var comp = drone.GetComp<CompDrone>();
                    if (!forced && !comp.autoRepair) return false;
                    if (!pawn.CanReserve(t, 1, -1, null, forced)) return false;
                    return true;
                }
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false) => JobMaker.MakeJob(InternalDefOf.VQE_RepairDrone, t);
    }
}
