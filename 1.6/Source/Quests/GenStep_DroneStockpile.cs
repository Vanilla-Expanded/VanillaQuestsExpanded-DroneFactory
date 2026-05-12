using System.Collections.Generic;
using System.Linq;
using VEF.Storyteller;
using Verse;
using Verse.AI.Group;

namespace VanillaQuestsExpandedDroneFactory
{
    public class GenStep_DroneStockpile : GenStep_Site
    {
        public override void Generate(Map map, GenStepParams parms)
        {
            var rects = BuildStructure(map, parms);
            var questPart = map.Parent.GetAssociatedPart<QuestPart_DroneStockpile>();
            var pawns = new List<Pawn>();
            foreach (var kind in questPart.defendingDrones)
            {
                if (CellFinder.TryFindRandomSpawnCellForPawnNear(rects.RandomElement().CenterCell, map, out var cell, 10))
                    pawns.Add(GenSpawn.Spawn(PawnGenerator.GeneratePawn(kind, map.ParentFaction), cell, map) as Pawn);
            }
            if (pawns.Any())
                LordMaker.MakeNewLord(map.ParentFaction, new LordJob_DefendPoint(map.Center), map, pawns);
        }
    }
}
