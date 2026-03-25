using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_Hardcrash : MentalState
    {
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            Find.LetterStack.ReceiveLetter("VQE_HardcrashLetter".Translate(pawn.Named("DRONE")), "VQE_HardcrashLetterDesc".Translate(pawn.Named("DRONE")), LetterDefOf.NegativeEvent, pawn);
        }
    }
}
