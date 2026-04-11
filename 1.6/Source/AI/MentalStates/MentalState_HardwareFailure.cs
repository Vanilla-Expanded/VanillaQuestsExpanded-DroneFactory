using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_HardwareFailure : MentalState
    {
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            Messages.Message("VQE_DroneMalfunction".Translate(pawn.Named("DRONE"), def.label) + "VQE_DroneMalfunction_Death".Translate(), pawn, MessageTypeDefOf.NegativeEvent);
        }
        public override void PostEnd()
        {
            base.PostEnd();
            var comp = pawn.GetComp<CompDrone>();
            comp.shouldKill = true;
        }
    }
}
