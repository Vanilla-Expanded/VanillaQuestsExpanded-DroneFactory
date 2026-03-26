using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HotSwappable]
    public class PawnColumnWorker_Lifespan : PawnColumnWorker
    {
        public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
        {
            var need = pawn.needs.TryGetNeed<Need_Lifespan>();
            need.DrawBarOnGUI(rect.ExpandedBy(-3f));
        }

        public override int GetMinWidth(PawnTable table) => 100;
    }
}
