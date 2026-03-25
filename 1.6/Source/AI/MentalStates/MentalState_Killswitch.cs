using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_Killswitch : MentalState_Berserk
    {
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            Find.LetterStack.ReceiveLetter("VQE_KillswitchLetter".Translate(pawn.Named("DRONE")), "VQE_KillswitchLetterDesc".Translate(pawn.Named("DRONE")), LetterDefOf.ThreatSmall, pawn);
        }
    }
}
