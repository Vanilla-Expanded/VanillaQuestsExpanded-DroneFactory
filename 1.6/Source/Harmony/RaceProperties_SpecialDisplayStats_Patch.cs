using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
	[HarmonyPatch(typeof(RaceProperties), nameof(RaceProperties.SpecialDisplayStats))]
	public static class RaceProperties_SpecialDisplayStats_Patch
	{
		private static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result, ThingDef parentDef)
		{
			if (parentDef != null && parentDef.comps != null && parentDef.comps.Any(c => c is CompProperties_Drone))
			{
				__result = __result.Where(entry => entry.LabelCap != "StatsReport_LifeExpectancy".Translate() && entry.LabelCap != "Diet".Translate() && entry.LabelCap != "Trainability".Translate() && entry.LabelCap != "StatsReport_AvailableTraining".Translate());
			}
			return __result;
		}
	}
}
