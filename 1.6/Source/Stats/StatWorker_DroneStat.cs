using Verse;
using RimWorld;

namespace VanillaQuestsExpandedDroneFactory
{
	public class StatWorker_DroneStat : StatWorker
	{
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			if (req.Thing is Pawn pawn)
			{
				return pawn.IsDrone();
			}
			return false;
		}
	}
}
