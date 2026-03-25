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
            Messages.Message("VQE_DroneHardwareFailure".Translate(pawn.Named("DRONE")), pawn, MessageTypeDefOf.NegativeEvent);
        }
        public override void PostEnd()
        {
            base.PostEnd();
            if (!pawn.Dead)
            {
                var core = pawn.health.hediffSet.GetNotMissingParts().FirstOrDefault(p => p.def == InternalDefOf.VQE_DroneCore);
                if (core != null) pawn.TakeDamage(new DamageInfo(DamageDefOf.Crush, 999f, 999f, -1f, null, core));
                else pawn.Kill(null);
            }
        }
    }
}
