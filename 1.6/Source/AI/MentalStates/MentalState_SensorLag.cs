using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_SensorLag : MentalState
    {
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            pawn.health.AddHediff(InternalDefOf.VQE_SensorLag_Hediff);
        }
        public override void PostEnd()
        {
            base.PostEnd();
            var h = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VQE_SensorLag_Hediff);
            if (h != null) pawn.health.RemoveHediff(h);
        }
    }
}
