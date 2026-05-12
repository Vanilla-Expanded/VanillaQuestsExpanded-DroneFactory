using System.Collections.Generic;
using RimWorld;
using VEF.Storyteller;
using Verse;
using Verse.AI.Group;

namespace VanillaQuestsExpandedDroneFactory
{
    public class GenStep_DroneFactory : GenStep_Site
    {
        public override void Generate(Map map, GenStepParams parms)
        {
            var rects = BuildStructure(map, parms);
            var faction = map.ParentFaction;
            var pawns = new List<Pawn>();
            var parmsMaker = new PawnGroupMakerParms
            {
                groupKind = PawnGroupKindDefOf.Combat,
                tile = map.Tile,
                faction = faction,
                points = parms.sitePart.parms.points,
                raidStrategy = RaidStrategyDefOf.ImmediateAttack
            };
            foreach (var pawn in PawnGroupMakerUtility.GeneratePawns(parmsMaker))
            {
                if (CellFinder.TryFindRandomSpawnCellForPawnNear(rects.RandomElement().CenterCell, map, out var cell, 10))
                {
                    GenSpawn.Spawn(pawn, cell, map);
                    pawns.Add(pawn);
                }
            }

            SpawnDrones(map, faction, pawns, rects);

            if (pawns.Any())
                LordMaker.MakeNewLord(faction, new LordJob_DefendPoint(map.Center, 20, addFleeToil: false), map, pawns);
        }

        private void SpawnDrones(Map map, Faction faction, List<Pawn> pawns, List<CellRect> rects)
        {
            var droneCounts = new (PawnKindDef kind, int min, int max)[]
            {
                (InternalDefOf.VQE_RaiderDrone, 3, 4),
                (InternalDefOf.VQE_BattleDrone, 4, 8),
                (InternalDefOf.VQE_HornetDrone, 2, 3)
            };

            foreach (var (kind, min, max) in droneCounts)
            {
                var count = Rand.RangeInclusive(min, max);
                for (int i = 0; i < count; i++)
                {
                    if (CellFinder.TryFindRandomSpawnCellForPawnNear(rects.RandomElement().CenterCell, map, out var cell, 10))
                    {
                        var drone = PawnGenerator.GeneratePawn(kind, faction);
                        GenSpawn.Spawn(drone, cell, map);
                        pawns.Add(drone);
                    }
                }
            }
        }
    }
}
