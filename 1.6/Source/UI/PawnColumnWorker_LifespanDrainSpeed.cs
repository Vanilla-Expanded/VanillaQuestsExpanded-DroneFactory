using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
	[HotSwappable]
	public class PawnColumnWorker_LifespanDrainSpeed : PawnColumnWorker
	{
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			float drainSpeed = pawn.GetStatValue(InternalDefOf.VQE_LifespanDrainSpeed);
			string label = drainSpeed.ToStringPercent();
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;

			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => GetTooltip(pawn), rect.GetHashCode()));
			}
		}

		private string GetTooltip(Pawn pawn)
		{
			StatDef stat = InternalDefOf.VQE_LifespanDrainSpeed;
			return stat.Worker.GetExplanationFull(StatRequest.For(pawn), stat.toStringNumberSense, pawn.GetStatValue(stat));
		}

		public override int GetMinWidth(PawnTable table) => 80;
	}
}
