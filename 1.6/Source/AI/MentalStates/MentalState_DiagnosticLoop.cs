using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_DiagnosticLoop : MentalState
    {
        private int recoveryTicks;

        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            pawn.jobs.StopAll();
            pawn.pather.StopDead();
            recoveryTicks = Rand.RangeInclusive(def.minTicksBeforeRecovery, def.maxTicksBeforeRecovery);
            pawn.stances.stunner.StunFor(recoveryTicks, null, showMote: true);
        }

        public override void PostEnd()
        {
            base.PostEnd();
            pawn.stances.stunner.StopStun();
        }

        public override void MentalStateTick(int delta)
        {
            base.MentalStateTick(delta);
            if (age >= recoveryTicks)
            {
                RecoverFromState();
                return;
            }
            if (pawn.IsHashIntervalTick(60))
            {
                pawn.Rotation = new Rot4((pawn.Rotation.AsInt + 1) % 4);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref recoveryTicks, "recoveryTicks");
        }
    }
}
