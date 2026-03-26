using Verse;
using RimWorld;
using System.Text;

namespace VanillaQuestsExpandedDroneFactory
{
	public class StatWorker_LifespanDrainSpeed : StatWorker_DroneStat
	{
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (!req.HasThing || !(req.Thing is Pawn pawn))
			{
				return base.GetValueUnfinalized(req, applyPostProcess);
			}

			float drainSpeed = base.GetValueUnfinalized(req, applyPostProcess);

			if (pawn.MentalStateDef != null)
			{
				var ext = pawn.MentalStateDef.GetModExtension<DroneMentalStateExtension>();
				if (ext != null)
				{
					drainSpeed *= ext.lifespanDrainMultiplier;
				}
			}

			return drainSpeed;
		}

		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder sb = new StringBuilder();

			float baseValueFor = GetBaseValueFor(req);
			if (baseValueFor != 0f || stat.showZeroBaseValue)
			{
				sb.AppendLine("StatsReport_BaseValue".Translate() + ": " + stat.ValueToString(baseValueFor, numberSense));
			}

			if (req.HasThing && req.Thing is Pawn pawn && pawn.MentalStateDef != null)
			{
				var ext = pawn.MentalStateDef.GetModExtension<DroneMentalStateExtension>();
				if (ext != null && ext.lifespanDrainMultiplier != 1f)
				{
					sb.AppendLine("VQE_Malfunction".Translate() + ": " + pawn.MentalStateDef.LabelCap + " x" + ext.lifespanDrainMultiplier.ToStringPercent());
				}
			}

			GetOffsetsAndFactorsExplanation(req, sb, baseValueFor);
			return sb.ToString();
		}
	}
}
