using Verse;
using RimWorld;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public abstract class WorkGiver_Drone : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.Touch;
        public override Danger MaxPathDanger(Pawn pawn) => Danger.Deadly;
        protected bool CanWorkOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (ForbidUtility.IsForbidden(t, pawn))
            {
                return false;
            }
            if (!pawn.CanReserveAndReach(t, PathEndMode.Touch, Danger.Deadly, ignoreOtherReservations: forced))
            {
                return false;
            }
            return true;
        }
    }
}
