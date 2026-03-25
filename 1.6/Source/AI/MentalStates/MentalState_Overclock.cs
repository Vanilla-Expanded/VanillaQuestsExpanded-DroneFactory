namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_Overclock : MentalState_DroneMalfunctionMessage
    {
        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            pawn.health.AddHediff(InternalDefOf.VQE_Overclock_Hediff);
        }
        public override void PostEnd()
        {
            base.PostEnd();
            var h = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VQE_Overclock_Hediff);
            if (h != null) pawn.health.RemoveHediff(h);
        }
    }
}
