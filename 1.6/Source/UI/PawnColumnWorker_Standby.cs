using System.Linq;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class PawnColumnWorker_Standby : PawnColumnWorker_Checkbox
    {
        protected override bool GetValue(Pawn pawn)
        {
            var comp = pawn.GetComp<CompDrone>();
            if (comp == null) return false;
            if (pawn.Spawned) return pawn.CurJobDef == InternalDefOf.VQE_EnterStandby;
            return true;
        }

        protected override void SetValue(Pawn pawn, bool value, PawnTable table)
        {
            var comp = pawn.GetComp<CompDrone>();
            if (comp != null)
            {
                if (value && pawn.Spawned) pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.VQE_EnterStandby, pawn));
                else if (!value && !pawn.Spawned)
                {
                    var buildings = Find.CurrentMap.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQE_DroneStandby);
                    var building = buildings.FirstOrDefault(b => b is Building_DroneStandby droneStandby && droneStandby.drone == pawn) as Building_DroneStandby;
                    if (building != null)
                    {
                        if (building.Map.designationManager.DesignationOn(building, InternalDefOf.VQE_ReactivateDrone_Designation) == null)
                        {
                            building.Map.designationManager.AddDesignation(new Designation(building, InternalDefOf.VQE_ReactivateDrone_Designation));
                        }
                    }
                }
            }
        }
    }
}
