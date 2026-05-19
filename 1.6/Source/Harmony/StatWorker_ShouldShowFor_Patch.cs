using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
	[HarmonyPatch(typeof(StatWorker), nameof(StatWorker.ShouldShowFor))]
	public static class StatWorker_ShouldShowFor_Patch
	{
		private static void Postfix(StatRequest req, StatDef ___stat, ref bool __result)
		{
			if (__result && req.Thing is Pawn pawn && pawn.IsDrone())
			{
				if (!___stat.showOnDrones || ___stat == StatDefOf.LifespanFactor)
				{
					__result = false;
				}
				
			}
		}
	}
}
