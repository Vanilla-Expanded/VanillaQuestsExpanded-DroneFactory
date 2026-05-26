using HarmonyLib;
using Verse;
using Verse.Profile;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(MemoryUtility), nameof(MemoryUtility.ClearAllMapsAndWorld))]
    public static class MemoryUtility_ClearAllMapsAndWorld_Patch
    {
        public static void Prefix()
        {
            Utils.ClearTransmitterCache();
            CompDrone.SpawnedDrones.Clear();
        }
    }
}