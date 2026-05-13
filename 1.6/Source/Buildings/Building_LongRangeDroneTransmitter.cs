using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HotSwappable]
    public class Building_LongRangeDroneTransmitter : Building, IHackable
    {
        public void OnLockedOut(Pawn pawn = null)
        {
        }
        public bool preventDisablingDrones;
        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (preventDisablingDrones is false)
            {
                DisableDrones();
            }
            base.Destroy(mode);
        }

        public void OnHacked(Pawn pawn = null)
        {
            DisableDrones();
        }

        private void DisableDrones()
        {
            var allDronesToKill = new HashSet<Pawn>();

            foreach (var dormantDrone in Map.listerThings.ThingsOfDef(InternalDefOf.VQED_DormantBattleDrone_Active).ToList())
            {
                var spawnedPawn = ActivateDormantDrone(dormantDrone);
                allDronesToKill.Add(spawnedPawn);
            }
            foreach (var dormantDrone in Map.listerThings.ThingsOfDef(InternalDefOf.VQED_DormantRaiderDrone_Active).ToList())
            {
                var spawnedPawn = ActivateDormantDrone(dormantDrone);
                allDronesToKill.Add(spawnedPawn);
            }
            foreach (var dormantDrone in Map.listerThings.ThingsOfDef(InternalDefOf.VQED_DormantHornetDrone_Active).ToList())
            {
                var spawnedPawn = ActivateDormantDrone(dormantDrone);
                allDronesToKill.Add(spawnedPawn);
            }

            var activeDrones = Map.mapPawns.AllPawnsSpawned.Where(x => StaticCollections.pawnCapacityLabels.ContainsKey(x.def) && x.Faction != Faction.OfPlayerSilentFail);
            allDronesToKill.AddRange(activeDrones);

            foreach (var drone in allDronesToKill)
            {
                KillDroneWithEMP(drone);
            }

            var thing = ThingMaker.MakeThing(InternalDefOf.VQE_DroneCore);
            GenPlace.TryPlaceThing(thing, InteractionCell, Map, ThingPlaceMode.Near);
        }

        private Pawn ActivateDormantDrone(Thing dormantDrone)
        {
            var extension = dormantDrone.def.GetModExtension<DroneTrapDetails>();
            var pawn = PawnGenerator.GeneratePawn(extension.droneSpawn, Faction.OfAncientsHostile);
            var cell = CellFinder.RandomClosewalkCellNear(dormantDrone.Position, Map, 1);
            GenSpawn.Spawn(pawn, cell, Map);
            dormantDrone.Destroy();
            return pawn;
        }

        private void KillDroneWithEMP(Pawn drone)
        {
            GenExplosion.DoExplosion(drone.Position, drone.Map, 1.5f, DamageDefOf.EMP, this);
            var damageInfo = new DamageInfo(DamageDefOf.EMP, 999f);
            drone.Kill(damageInfo);
        }
    }
}
