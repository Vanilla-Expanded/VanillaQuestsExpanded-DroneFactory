using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using VEF.Buildings;
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
            var site = map.Parent as Site;
            var sitePart = site.parts.FirstOrDefault(x => x.def == InternalDefOf.VQE_DroneStockpile);
            var transmitter = map.listerThings.ThingsOfDef(InternalDefOf.VQE_LongRangeDroneTransmitter).FirstOrDefault() as Building_LongRangeDroneTransmitter;

            var pos = transmitter.Position;
            var rot = transmitter.Rotation;
            transmitter.preventDisablingDrones = true;
            transmitter.Destroy();
            var newTransmitter = GenSpawn.Spawn(sitePart.conditionCauser, pos, map, rot);
            sitePart.conditionCauserWasSpawned = true;
            var compBounce = newTransmitter.TryGetComp<CompBouncingArrow>();
            compBounce.doBouncingArrow = true;
            var questPart = map.Parent.GetAssociatedPart<QuestPart_DroneStockpile>();
            var pawns = new List<Pawn>();

            foreach (var kind in questPart.defendingDrones)
            {
                if (CellFinder.TryFindRandomSpawnCellForPawnNear(rects.RandomElement().CenterCell, map, out var cell, 10))
                    pawns.Add(GenSpawn.Spawn(PawnGenerator.GeneratePawn(kind, map.ParentFaction), cell, map) as Pawn);
            }

            LordMaker.MakeNewLord(map.ParentFaction, new LordJob_DefendPoint(map.Center), map, pawns);
        }
    }
}
