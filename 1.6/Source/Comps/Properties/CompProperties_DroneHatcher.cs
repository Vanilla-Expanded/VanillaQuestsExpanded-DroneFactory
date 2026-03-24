
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class CompProperties_DroneHatcher : CompProperties
    {

        public PawnKindDef hatcherKindDef;
       
        public CompProperties_DroneHatcher()
        {

            this.compClass = typeof(CompDroneHatcher);
        }
    }
}

