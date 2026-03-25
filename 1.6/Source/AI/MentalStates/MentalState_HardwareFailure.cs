using Verse;
using RimWorld;
using Verse.AI;
using System.Linq;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_HardwareFailure : MentalState
    {
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            Messages.Message("VQE_DroneMalfunction_Death".Translate(pawn.Named("DRONE"), pawn.def.label), pawn, MessageTypeDefOf.NegativeEvent);
        }
        public override void PostEnd()
        {
            base.PostEnd();
            if (!pawn.Dead)
            {
                pawn.DestroyCore();
            }
        }
    }
}
