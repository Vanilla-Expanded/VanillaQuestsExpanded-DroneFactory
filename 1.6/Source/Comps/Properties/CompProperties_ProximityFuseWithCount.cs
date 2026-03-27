
using System.Collections.Generic;
using RimWorld;
using Verse;
namespace VanillaQuestsExpandedDroneFactory

{
    public class CompProperties_ProximityFuseWithCount : CompProperties
    {
        public ThingDef target;

        public float radius;

        public int amount;

        public CompProperties_ProximityFuseWithCount()
        {
            compClass = typeof(CompProximityFuseWithCount);
        }

       
    }
}