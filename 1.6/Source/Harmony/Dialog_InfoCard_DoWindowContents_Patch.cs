using HarmonyLib;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(Dialog_InfoCard), "DoWindowContents")]
    public static class Dialog_InfoCard_DoWindowContents_Patch
    {
        public static void Prefix(ref Thing ___thing)
        {
            if (___thing is Building_DroneStandby building)
            {
                ___thing = building.drone;
            }
        }
    }
}
