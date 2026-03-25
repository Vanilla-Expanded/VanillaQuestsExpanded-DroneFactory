using Verse;
using RimWorld;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;

namespace VanillaQuestsExpandedDroneFactory
{
    public class WorkGiver_ShutdownDrone : WorkGiver_Scanner
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn) => pawn.Map.designationManager.SpawnedDesignationsOfDef(InternalDefOf.VQE_ShutdownDrone_Designation).Select(d => d.target.Thing);
        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false) => JobMaker.MakeJob(InternalDefOf.VQE_ShutdownDrone, t);
    }
}
