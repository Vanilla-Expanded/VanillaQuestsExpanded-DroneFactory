using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace VanillaQuestsExpandedDroneFactory
{
    public class CompProperties_Drone : CompProperties
    {
        public CompProperties_Drone() => compClass = typeof(CompDrone);
    }

    public class CompDrone : ThingComp
    {
        public bool autoRepair = true;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            var p = (Pawn)parent;
            if (p.playerSettings == null) p.playerSettings = new Pawn_PlayerSettings(p);
            if (p.drafter == null) p.drafter = new Pawn_DraftController(p);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref autoRepair, "autoRepair", true);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var g in base.CompGetGizmosExtra()) yield return g;

            var pawn = (Pawn)parent;
            if (pawn.Faction != Faction.OfPlayer) yield break;

            yield return new Command_Action
            {
                defaultLabel = "VQE_Shutdown".Translate(),
                defaultDesc = "VQE_ShutdownDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Gizmos/ShutDown_Designator"),
                action = () => pawn.Map.designationManager.AddDesignation(new Designation(pawn, InternalDefOf.VQE_ShutdownDrone_Designation))
            };

            var cmd = new Command_Action
            {
                defaultLabel = "VQE_Standby".Translate(),
                defaultDesc = "VQE_StandbyDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Gizmos/StandBy_Gizmo"),
                action = () => pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.VQE_EnterStandby, pawn))
            };

            if (pawn.MentalStateDef != null)
            {
                var ext = pawn.MentalStateDef.GetModExtension<DroneMentalStateExtension>();
                if (ext?.disableDraftGizmos ?? false)
                {
                    cmd.Disable(ext.draftDisableMessage.Translate());
                }
            }

            yield return cmd;
        }
    }
}
