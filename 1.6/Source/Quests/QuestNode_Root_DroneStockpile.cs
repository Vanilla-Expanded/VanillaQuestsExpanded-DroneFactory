using System.Collections.Generic;
using RimWorld;
using RimWorld.QuestGen;
using VEF.Storyteller;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HotSwappable]
    public class QuestNode_Root_DroneStockpile : QuestNode_Site
    {
        protected override void RunInt()
        {
            if (!PrepareQuest(out var playerMap, out var points, out var tile, out var slate))
                return;

            var site = GenerateSite(points, tile, Faction.OfAncientsHostile, slate,
                out var siteMapGeneratedSignal, out var siteMapRemovedSignal,
                failWhenMapRemoved: true, timeoutTicks: 0);

            if (site == null) return;

            var defendingDrones = GenerateDefendingDrones(points);
            slate.Set("ListOfEnemies", QuestUtils.FormatPawnListToString(defendingDrones));

            var outSignalComplete = QuestGenUtility.HardcodedSignalWithQuestID("transmitterDestroyed");
            var questPart_Stockpile = new QuestPart_DroneStockpile
            {
                mapParent = site,
                inSignalEnable = siteMapGeneratedSignal,
                defendingDrones = defendingDrones,
                outSignalComplete = outSignalComplete,
                playerMap = playerMap,
            };
            QuestGen.quest.AddPart(questPart_Stockpile);

            QuestGen.quest.End(QuestEndOutcome.Success, 0, null, outSignalComplete, sendStandardLetter: true);
        }

        private static List<PawnKindDef> GenerateDefendingDrones(float points)
        {
            var defending = new List<PawnKindDef>();
            var available = new List<PawnKindDef> { InternalDefOf.VQE_BattleDrone, InternalDefOf.VQE_RaiderDrone };
            float spent = 0f;
            while (spent < points)
            {
                var kind = available.RandomElement();
                spent += kind.combatPower;
                defending.Add(kind);
            }
            return defending;
        }
    }
}
