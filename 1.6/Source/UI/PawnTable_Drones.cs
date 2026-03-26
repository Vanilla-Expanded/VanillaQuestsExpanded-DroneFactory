using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class PawnTable_Drones : PawnTable
    {
        public PawnTable_Drones(PawnTableDef def, System.Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight) { }
    }
}
