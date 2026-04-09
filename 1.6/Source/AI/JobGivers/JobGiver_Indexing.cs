using Verse;
using RimWorld;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobGiver_Indexing : JobGiver_AIFollowPawn
    {
        protected override int FollowJobExpireInterval => 60;
        protected override Pawn GetFollowee(Pawn pawn)
        {
            if (pawn.MentalState is not MentalState_Indexing mentalState) return null;
            return mentalState.target;
        }

        protected override float GetRadius(Pawn pawn)
        {
            return 1.5f;
        }
    }
}
