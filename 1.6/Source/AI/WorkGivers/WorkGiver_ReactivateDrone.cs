using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class WorkGiver_ReactivateDrone : WorkGiver_Drone
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQE_DroneStandby).OfType<Building_DroneStandby>();
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t is not Building_DroneStandby building)
            {
                return null;
            }
            if (building.drone.Faction != pawn.Faction)
            {
                return null;
            }
            if (t.Map.designationManager.DesignationOn(t, InternalDefOf.VQE_ReactivateDrone_Designation) == null)
            {
                return null;
            }
            if (!CanWorkOnThing(pawn, t, forced))
            {
                return null;
            }
            return JobMaker.MakeJob(InternalDefOf.VQE_ReactivateDrone, t);
        }
    }
}
