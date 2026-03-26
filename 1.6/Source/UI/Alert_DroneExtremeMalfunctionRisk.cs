using System.Linq;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Alert_DroneExtremeMalfunctionRisk : Alert_Critical
    {
        public Alert_DroneExtremeMalfunctionRisk()
        {
            defaultLabel = "VQE_ExtremeMalfunctionRisk".Translate();
        }

        public override AlertReport GetReport()
        {
            var culprits = Find.CurrentMap?.mapPawns.PawnsInFaction(Faction.OfPlayer)
                .Where(p => p.IsDrone() && p.needs.TryGetNeed<Need_Lifespan>().CurLevelPercentage < 0.05f).ToList();
            if (culprits.NullOrEmpty()) return false;
            return AlertReport.CulpritsAre(culprits);
        }

        public override TaggedString GetExplanation()
        {
            var culprits = Find.CurrentMap?.mapPawns.PawnsInFaction(Faction.OfPlayer)
                .Where(p => p.IsDrone() && p.needs.TryGetNeed<Need_Lifespan>().CurLevelPercentage < 0.05f).ToList();
            return string.Format("VQE_ExtremeMalfunctionRiskDesc".Translate(), string.Join("\n", culprits.Select(p => " - " + p.LabelShortCap)));
        }
    }
}
