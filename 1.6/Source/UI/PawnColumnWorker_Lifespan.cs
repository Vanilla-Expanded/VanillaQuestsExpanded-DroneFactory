using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class PawnColumnWorker_Lifespan : PawnColumnWorker
    {
        public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
        {
            var need = pawn.needs?.TryGetNeed<Need_Lifespan>();
            if (need != null)
            {
                float pct = need.CurLevelPercentage;
                var barRect = new Rect(rect.x, rect.y + 2f, rect.width, rect.height - 4f);
                Widgets.FillableBar(barRect, pct);
                DrawThreshold(barRect, 0.35f);
                DrawThreshold(barRect, 0.20f);
                DrawThreshold(barRect, 0.05f);
            }
        }

        private void DrawThreshold(Rect barRect, float pct)
        {
            var rect = new Rect(barRect.x + barRect.width * pct - 1f, barRect.y, 2f, barRect.height);
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, rect.height), BaseContent.BlackTex);
        }

        public override int GetMinWidth(PawnTable table) => 100;
    }
}
