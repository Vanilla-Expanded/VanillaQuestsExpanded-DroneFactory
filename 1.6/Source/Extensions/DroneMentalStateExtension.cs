using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class DroneMentalStateExtension : DefModExtension
    {
        public float lifespanDrainMultiplier = 1f;
        public MalfunctionTier malfunctionTier = MalfunctionTier.None;
        public bool reverseNetworkRestriction = false;
        public bool disableDraftGizmos = false;
        public string draftDisableMessage = null;
    }

    public enum MalfunctionTier
    {
        None,
        Minor,
        Major,
        Extreme
    }
}
