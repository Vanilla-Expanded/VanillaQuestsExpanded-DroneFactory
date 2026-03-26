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
    [HotSwappable]
    public class CompDrone : ThingComp
    {
        public bool autoRepair = true;
        public bool shouldKill;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            var p = (Pawn)parent;
            if (p.playerSettings == null) p.playerSettings = new Pawn_PlayerSettings(p);
            if (p.drafter == null) p.drafter = new Pawn_DraftController(p);
        }

        public override void CompTick()
        {
            base.CompTick();
            if (shouldKill)
            {
                var pawn = parent as Pawn;
                pawn.Kill(null);
                shouldKill = false;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref autoRepair, "autoRepair", true);
        }

        public override bool WantHoldWeapon(Pawn pawn)
        {
            return pawn.kindDef == InternalDefOf.VQE_TurretDrone;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            var pawn = (Pawn)parent;
            if (DebugSettings.ShowDevGizmos)
            {
                if (pawn.Faction != Faction.OfPlayer)
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "DEV: Become player drone",
                        action = () => pawn.SetFaction(Faction.OfPlayer)
                    };
                }
                var lifespan = pawn.needs.TryGetNeed<Need_Lifespan>();
                if (lifespan != null)
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "DEV: Set lifespan: Max",
                        action = () => lifespan.CurLevel = lifespan.MaxLevel
                    };
                    yield return new Command_Action
                    {
                        defaultLabel = "DEV: Set lifespan: Minor threshold",
                        action = () => lifespan.CurLevelPercentage = 0.35f
                    };
                    yield return new Command_Action
                    {
                        defaultLabel = "DEV: Set lifespan: Major threshold",
                        action = () => lifespan.CurLevelPercentage = 0.20f
                    };
                    yield return new Command_Action
                    {
                        defaultLabel = "DEV: Set lifespan: Extreme threshold",
                        action = () => lifespan.CurLevelPercentage = 0.05f
                    };
                    yield return new Command_Action
                    {
                        defaultLabel = "DEV: Set lifespan: 0%",
                        action = () => lifespan.CurLevel = 0f
                    };
                }
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Trigger malfunction",
                    action = () =>
                    {
                        var options = new List<FloatMenuOption>();
                        foreach (var def in DefDatabase<MentalStateDef>.AllDefs)
                        {
                            if (def.HasModExtension<DroneMentalStateExtension>())
                            {
                                options.Add(new FloatMenuOption(def.LabelCap, () =>
                                {
                                    pawn.mindState.mentalStateHandler.CurState?.RecoverFromState();
                                    pawn.mindState.mentalStateHandler.TryStartMentalState(def);
                                }));
                            }
                        }
                        Find.WindowStack.Add(new FloatMenu(options));
                    }
                };
                if (pawn.InMentalState && pawn.MentalStateDef.HasModExtension<DroneMentalStateExtension>())
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "DEV: End malfunction",
                        action = () => pawn.mindState.mentalStateHandler.CurState?.RecoverFromState()
                    };
                }
            }
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
                defaultLabel = "VQE_EnterStandby".Translate(),
                defaultDesc = "VQE_EnterStandbyDesc".Translate(),
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
