using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_SelfDecommission : MentalState_GiveUpExit
    {
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            Find.LetterStack.ReceiveLetter("VQE_SelfDecommissionLetter".Translate(pawn.Named("DRONE")), "VQE_SelfDecommissionLetterDesc".Translate(pawn.Named("DRONE")), LetterDefOf.NegativeEvent, pawn);
        }
    }
}
