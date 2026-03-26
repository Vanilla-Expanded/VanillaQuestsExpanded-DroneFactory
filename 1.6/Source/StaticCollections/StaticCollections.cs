
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;


namespace VanillaQuestsExpandedDroneFactory
{
    [StaticConstructorOnStartup]
    public static class StaticCollections
    {
        public static Dictionary<ThingDef, Dictionary<PawnCapacityDef, string>> pawnCapacityLabels = new Dictionary<ThingDef, Dictionary<PawnCapacityDef, string>>();

        static StaticCollections()
        {

            List<ThingDef> drones = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.GetModExtension<DroneCapacityLabelsExtension>() != null).ToList();

            foreach (ThingDef drone in drones)
            {
                pawnCapacityLabels[drone] = drone.GetModExtension<DroneCapacityLabelsExtension>()?.capacityLabels;
            }
      

        }

    }
}
