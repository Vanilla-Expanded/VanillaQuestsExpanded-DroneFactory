using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;

namespace VanillaQuestsExpandedDroneFactory
{
    public class PawnColumnWorker_Standby : PawnColumnWorker_Checkbox
    {
        public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
        {
            if (!pawn.IsDrone()) return;

            var gizmoDisableReport = Utils.ShouldDisableDroneGizmos(pawn);
            if (!gizmoDisableReport.Accepted)
            {
                rect.xMin += (rect.width - 24f) / 2f;
                rect.yMin += (rect.height - 24f) / 2f;
                bool dummyValue = GetValue(pawn);
                Widgets.Checkbox(rect.position, ref dummyValue, 24f, disabled: true);
                if (!gizmoDisableReport.Reason.NullOrEmpty())
                {
                    TooltipHandler.TipRegion(rect, gizmoDisableReport.Reason.CapitalizeFirst());
                }
                return;
            }

            base.DoCell(rect, pawn, table);
        }

        protected override bool GetValue(Pawn pawn)
        {
            var comp = pawn.GetComp<CompDrone>();
            if (pawn.Spawned) return pawn.CurJobDef == InternalDefOf.VQE_EnterStandby;
            var buildings = Find.CurrentMap?.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQE_DroneStandby);
            var building = buildings?.FirstOrDefault(b => b is Building_DroneStandby droneStandby && droneStandby.drone == pawn) as Building_DroneStandby;
            if (building != null)
            {
                return building.Map.designationManager.DesignationOn(building, InternalDefOf.VQE_ReactivateDrone_Designation) == null;
            }
            return true;
        }

        protected override void SetValue(Pawn pawn, bool value, PawnTable table)
        {
            var comp = pawn.GetComp<CompDrone>();
            if (comp != null)
            {
                if (pawn.Spawned)
                {
                    if (value) pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.VQE_EnterStandby, pawn));
                    else if (pawn.CurJobDef == InternalDefOf.VQE_EnterStandby) pawn.jobs.StopAll();
                }
                else
                {
                    var buildings = Find.CurrentMap.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQE_DroneStandby);
                    var building = buildings.FirstOrDefault(b => b is Building_DroneStandby droneStandby && droneStandby.drone == pawn) as Building_DroneStandby;
                    if (building != null)
                    {
                        var designation = building.Map.designationManager.DesignationOn(building, InternalDefOf.VQE_ReactivateDrone_Designation);
                        if (value)
                        {
                            designation?.Delete();
                        }
                        else if (designation == null)
                        {
                            building.Map.designationManager.AddDesignation(new Designation(building, InternalDefOf.VQE_ReactivateDrone_Designation));
                        }
                    }
                }
            }
        }

        public override int GetOptimalWidth(PawnTable table)
        {
            return base.GetOptimalWidth(table) + 10;
        }
    }
}
