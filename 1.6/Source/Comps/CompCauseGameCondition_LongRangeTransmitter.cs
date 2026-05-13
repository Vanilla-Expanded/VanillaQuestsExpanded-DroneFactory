using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class CompCauseGameCondition_LongRangeTransmitter : CompCauseGameCondition
    {
        private CompHackable hackableComp;

        public override bool Active => !HackableComp.IsHacked;

        public CompHackable HackableComp => hackableComp ?? (hackableComp = parent.GetComp<CompHackable>());
    }
}