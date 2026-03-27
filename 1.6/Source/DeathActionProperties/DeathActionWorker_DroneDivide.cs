
using RimWorld;
using Verse;
using Verse.AI.Group;
using Verse.Noise;

namespace VanillaQuestsExpandedDroneFactory

{
    public class DeathActionWorker_DroneDivide : DeathActionWorker
    {
        public DeathActionProperties_DroneDivide Props => (DeathActionProperties_DroneDivide)props;

        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
           
            Pawn innerPawn = corpse.InnerPawn;
            if (innerPawn == null)
            {
                return;
            }
           
            for (int i = 0; i < 2; i++)
            {
                PawnKindDef kind = Props.dividePawnKindOptions[i];
                Faction faction = corpse.InnerPawn.Faction;
                float? fixedBiologicalAge = 0f;
                Pawn child = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, PawnGenerationContext.NonPlayer, fixedBiologicalAge: fixedBiologicalAge));
                SpawnPawn(child, innerPawn, corpse.PositionHeld, corpse.MapHeld, prevLord);
            }

            for (int i = 0; i < 3; i++)
            {
                IntVec3 c;
                CellFinder.TryFindRandomReachableNearbyCell(corpse.PositionHeld, corpse.MapHeld, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);

                FilthMaker.TryMakeFilth(c, corpse.MapHeld, ThingDefOf.Filth_MachineBits);

            }
          
            corpse.Destroy();
        }

        private void SpawnPawn(Pawn child, Pawn parent, IntVec3 position, Map map, Lord lord)
        {
            GenSpawn.Spawn(child, position, map, WipeMode.VanishOrMoveAside);
            lord?.AddPawn(child);
           
            FleshbeastUtility.SpawnPawnAsFlyer(child, map, position);
        }
    }
}