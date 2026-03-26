using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_HoardingLoop : MentalState
    {
        public HashSet<int> hoardedItemIds;

        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            hoardedItemIds = new HashSet<int>();
        }

        public override void PostEnd()
        {
            base.PostEnd();
            hoardedItemIds?.Clear();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref hoardedItemIds, "hoardedItemIds", LookMode.Value);
        }
    }
}
