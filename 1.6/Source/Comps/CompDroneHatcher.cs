
using Verse;
using System;
using RimWorld.Planet;
using RimWorld;

namespace VanillaQuestsExpandedDroneFactory
{
    class CompDroneHatcher : ThingComp
    {  

        public CompProperties_DroneHatcher Props
        {
            get
            {
                return (CompProperties_DroneHatcher)this.props;
            }
        }

        public override void CompTick()
        {
            if (!(parent.ParentHolder is Pawn_CarryTracker) && this.parent.Map != null)
            {
                this.Hatch();
            }
        }


        public void Hatch()
        {
            try
            {
                PawnGenerationRequest request = new PawnGenerationRequest(Props.hatcherKindDef, Faction.OfPlayerSilentFail, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn);
                (parent.ParentHolder as Pawn_CarryTracker)?.TryDropCarriedThing(parent.PositionHeld, ThingPlaceMode.Near, out var _);
               
                Pawn pawn = PawnGenerator.GeneratePawn(request);
                PawnUtility.TrySpawnHatchedOrBornPawn(pawn, parent);
                
            }
            finally
            {
                parent.Destroy();
            }
        }

    }
}

