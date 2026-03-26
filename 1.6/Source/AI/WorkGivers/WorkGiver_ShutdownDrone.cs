using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;

namespace VanillaQuestsExpandedDroneFactory
{
    public class WorkGiver_ShutdownDrone : WorkGiver_Drone
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn) => pawn.Map.designationManager.SpawnedDesignationsOfDef(InternalDefOf.VQE_ShutdownDrone_Designation).Select(d => d.target.Thing);

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t.Map.designationManager.DesignationOn(t, InternalDefOf.VQE_ShutdownDrone_Designation) == null)
            {
                return null;
            }
            if (!CanWorkOnThing(pawn, t, forced))
            {
                return null;
            }
            return JobMaker.MakeJob(InternalDefOf.VQE_ShutdownDrone, t);
        }
    }
}
