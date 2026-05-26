using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace VanillaQuestsExpandedDroneFactory
{
    public class CompProperties_Drone : CompProperties
    {
        public bool requiresTransmitter;
        public CompProperties_Drone() => compClass = typeof(CompDrone);
    }

    [HotSwappable]
    public class CompDrone : ThingComp
    {
        public static readonly Dictionary<Pawn, CompDrone> SpawnedDrones = new();
        public CompProperties_Drone Props => (CompProperties_Drone)props;
        public bool autoRepair = true;
        public bool shouldKill;
        public bool isPlayerDrone = false;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            var p = (Pawn)parent;
            SpawnedDrones[p] = this;
            if (p.playerSettings == null) p.playerSettings = new Pawn_PlayerSettings(p);
            if (p.drafter == null) p.drafter = new Pawn_DraftController(p);
        }

        public override void CompTick()
        {
            base.CompTick();
            var pawn = (Pawn)parent;
            if (shouldKill)
            {
                pawn.Kill(null);
                shouldKill = false;
                return;
            }
            if (pawn.IsHashIntervalTick(60) && Props.requiresTransmitter && pawn.CurJobDef == InternalDefOf.VQE_StandOutOfTransmitterRange && Utils.IsWithinTransmitter(pawn.Position, pawn.Map))
            {
                pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
            }
        }

        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            base.PostDeSpawn(map, mode);
            SpawnedDrones.Remove((Pawn)parent);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref autoRepair, "autoRepair", true);
            Scribe_Values.Look(ref isPlayerDrone, "isPlayerDrone", false);

        }

        public override bool WantHoldWeapon(Pawn pawn)
        {
            return pawn.kindDef == InternalDefOf.VQE_TurretDrone;
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            var drone = (Pawn)parent;
            if (selPawn.IsColonist && selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                if (!drone.IsWithinTransmitter())
                {
                    var transmitter = GenClosest.ClosestThingReachable(selPawn.Position, selPawn.Map,
                        ThingRequest.ForDef(InternalDefOf.VQE_DroneTransmitter), PathEndMode.Touch, TraverseParms.For(selPawn),
                        validator: t => t.TryGetComp<CompPowerTrader>().PowerOn);

                    if (transmitter != null && selPawn.CanReserveAndReach(drone, PathEndMode.Touch, Danger.Deadly))
                    {
                        var targetCell = FindClosestTransmitterCell(transmitter, drone.Position);
                        yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(
                            "VQE_CarryDroneToTransmitter".Translate(drone.LabelShort), delegate
                        {
                            var job = JobMaker.MakeJob(InternalDefOf.VQE_CarryDroneToTransmitter, drone, transmitter, targetCell);
                            job.count = 1;
                            selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        }), selPawn, drone);
                    }
                }
            }
        }

        private LocalTargetInfo FindClosestTransmitterCell(Thing transmitter, IntVec3 dronePos)
        {
            var map = transmitter.Map;
            var result = transmitter.Position;
            var minDistanceSq = float.MaxValue;

            foreach (var cell in GenRadial.RadialCellsAround(transmitter.Position, Utils.TransmitterRadius, true))
            {
                if (cell.InBounds(map) && cell.Walkable(map))
                {
                    var distanceSq = cell.DistanceToSquared(dronePos);
                    if (distanceSq < minDistanceSq)
                    {
                        minDistanceSq = distanceSq;
                        result = cell;
                    }
                }
            }

            return result;
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            var drone = (Pawn)parent;
            if (drone.Drafted && Utils.RequiresTransmitter(drone))
            {
                GenDraw.DrawFieldEdges(Utils.GetTransmitterCells(drone.Map));
            }
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

            if (pawn.Map.designationManager.DesignationOn(pawn, InternalDefOf.VQE_ShutdownDrone_Designation) == null)
            {
                yield return new Command_Action
                {
                    defaultLabel = "VQE_Shutdown".Translate(),
                    defaultDesc = "VQE_ShutdownDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Gizmos/ShutDown_Designator"),
                    action = () => pawn.Map.designationManager.AddDesignation(new Designation(pawn, InternalDefOf.VQE_ShutdownDrone_Designation))
                };
            }

            var cmd = new Command_Action
            {
                defaultLabel = "VQE_EnterStandby".Translate(),
                defaultDesc = "VQE_EnterStandbyDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Gizmos/StandBy_Gizmo"),
                action = () => pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.VQE_EnterStandby, pawn))
            };

            if (!pawn.IsWithinTransmitter())
            {
                cmd.Disable("VQE_OutsideTransmitterRange".Translate());
            }
            else if (pawn.MentalStateDef != null)
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
