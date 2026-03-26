using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_FluidLeak : MentalState
    {
        public override void MentalStateTick(int delta)
        {
            base.MentalStateTick(delta);
            if (pawn.IsHashIntervalTick(60) && pawn.Position.Walkable(pawn.Map))
            {
                FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, InternalDefOf.VQE_MachiningFluid);
            }
        }
    }
}
