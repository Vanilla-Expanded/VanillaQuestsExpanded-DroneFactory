
using Verse;
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
                PawnGenerationRequest request = new PawnGenerationRequest(Props.hatcherKindDef, Faction.OfPlayerSilentFail, 
                    PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: true);
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

