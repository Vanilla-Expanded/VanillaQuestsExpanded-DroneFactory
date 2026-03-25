using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_DiagnosticLoop : MentalState
    {
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            pawn.stances.stunner.StunFor(int.MaxValue, null, showMote: true);
        }

        public override void MentalStateTick(int delta)
        {
            base.MentalStateTick(delta);
            if (pawn.IsHashIntervalTick(60))
            {
                pawn.Rotation = new Rot4((pawn.Rotation.AsInt + 1) % 4);
            }
        }
    }
}
