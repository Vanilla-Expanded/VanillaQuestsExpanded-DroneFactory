
using System.Collections.Generic;
using Verse;
namespace VanillaQuestsExpandedDroneFactory

{
    public class DeathActionProperties_DroneDivide : DeathActionProperties
    {
        public List<PawnKindDef> dividePawnKindOptions = new List<PawnKindDef>();

        public DeathActionProperties_DroneDivide()
        {
            workerClass = typeof(DeathActionWorker_DroneDivide);
        }

      
    }
}