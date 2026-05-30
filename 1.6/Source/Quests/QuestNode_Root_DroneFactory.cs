using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.QuestGen;
using VEF.Storyteller;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HotSwappable]
    public class QuestNode_Root_DroneFactory : QuestNode_Site
    {
        protected override void RunInt()
        {
            if (!PrepareQuest(out var map, out var points, out var tile, out var slate))
                return;

            var pirateFaction = Find.FactionManager.AllFactions
                .Where(f => f.IsPirateOrEnemy())
                .RandomElementWithFallback();
            if (pirateFaction == null) return;

            var site = GenerateSite(points, tile, pirateFaction, slate,
                out var siteMapGeneratedSignal, out var siteMapRemovedSignal,
                failWhenMapRemoved: false, timeoutTicks: 0);
            if (site == null) return;

            var askerFaction = Find.FactionManager.AllFactions
                .Where(f => f != Faction.OfPlayer && f.IsPirateOrEnemy() is false && f.def.humanlikeFaction && f.leader != null)
                .RandomElementWithFallback();
            if (askerFaction is null) return;

            slate.Set("asker", askerFaction.leader);

            var defendingDrones = new List<PawnKindDef>();
            var count = Rand.RangeInclusive(3, 4);
            for (int i = 0; i < count; i++) defendingDrones.Add(InternalDefOf.VQE_RaiderDrone);
            count = Rand.RangeInclusive(4, 8);
            for (int i = 0; i < count; i++) defendingDrones.Add(InternalDefOf.VQE_BattleDrone);
            count = Rand.RangeInclusive(2, 3);
            for (int i = 0; i < count; i++) defendingDrones.Add(InternalDefOf.VQE_HornetDrone);
            slate.Set("ListOfEnemies", QuestUtils.FormatPawnListToString(defendingDrones));

            var outSignalComplete = QuestGenUtility.HardcodedSignalWithQuestID("allEnemiesDefeated");
            var questPart = new QuestPart_DroneFactory
            {
                mapParent = site,
                inSignalEnable = siteMapGeneratedSignal,
                outSignalComplete = outSignalComplete
            };
            QuestGen.quest.AddPart(questPart);

            var rewardParams = new RewardsGeneratorParams
            {
                allowGoodwill = true,
                allowRoyalFavor = true,
                giverFaction = askerFaction,
            };
            QuestGen.quest.GiveRewards(rewardParams, outSignalComplete,
                addCampLootReward: true, asker: askerFaction?.leader, useDifficultyFactor: true);

            QuestGen.quest.End(QuestEndOutcome.Success, 0, null, outSignalComplete,
                sendStandardLetter: true);
        }
    }
}
