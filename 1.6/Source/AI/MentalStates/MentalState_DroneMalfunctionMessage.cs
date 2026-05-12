using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_DroneMalfunctionMessage : MentalState
    {
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            if (pawn.Faction == Faction.OfPlayer)
                Messages.Message("VQE_DroneMalfunction".Translate(pawn.Named("DRONE"), def.label), pawn, MessageTypeDefOf.NegativeEvent);
        }
    }
}
