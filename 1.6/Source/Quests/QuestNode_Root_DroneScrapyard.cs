using RimWorld;
using RimWorld.QuestGen;
using System.Collections.Generic;
using Verse;
using VEF.Storyteller;
using System.Linq;

namespace VanillaQuestsExpandedDroneFactory
{
    [HotSwappable]
    public class QuestNode_Root_DroneScrapyard : QuestNode_Site
    {
        private const float MaxQuestPoints = 5000f;

        protected override void RunInt()
        {
            if (PrepareQuest(out var map, out var points, out var tile, out var slate))
            {
                var askerFaction = Find.FactionManager.AllFactions.Where(x => x != Faction.OfPlayer && x.def.humanlikeFaction && x.Hidden is false && x.IsPirateOrEnemy() is false && x.leader != null).RandomElement();
                if (askerFaction is null) return;
                var site = GenerateSite(points, tile, Faction.OfAncientsHostile, slate, out var siteMapGeneratedSignal, out var siteMapRemovedSignal, failWhenMapRemoved: false, 15 * GenDate.TicksPerDay);
                if (site != null)
                {
                    var defendingDrones = new List<PawnKindDef>();
                    GenerateDefendingDrones(points, defendingDrones);

                    var outSignalComplete = QuestGenUtility.HardcodedSignalWithQuestID("allDronesDefeated");

                    var questPart_DefeatAllEnemies = new QuestPart_AllDronesDefeated
                    {
                        mapParent = site,
                        inSignalEnable = siteMapGeneratedSignal,
                        outSignalComplete = outSignalComplete,
                        defendingDrones = defendingDrones
                    };
                    QuestGen.quest.AddPart(questPart_DefeatAllEnemies);

                    QuestGen.slate.Set("defendingDrones", QuestUtils.FormatPawnListToString(defendingDrones));
                    QuestGen.slate.Set("asker", askerFaction.leader);

                    var rewardParams = new RewardsGeneratorParams { allowGoodwill = true, allowRoyalFavor = true, giverFaction = askerFaction };
                    QuestGen.quest.GiveRewards(rewardParams, outSignalComplete, addCampLootReward: true, asker: askerFaction.leader, useDifficultyFactor: true);

                    QuestGen.quest.End(QuestEndOutcome.Success, 0, null, outSignalComplete, sendStandardLetter: true);
                }
            }
        }

        private void GenerateDefendingDrones(float points, List<PawnKindDef> defendingDrones)
        {
            var availableDrones = new List<PawnKindDef> { InternalDefOf.VQE_ShufflerDrone, InternalDefOf.VQE_BattleDrone, InternalDefOf.VQE_RaiderDrone };
            float spentPoints = 0;
            float targetPoints = points > MaxQuestPoints ? MaxQuestPoints : points;
            while (spentPoints < targetPoints)
            {
                var kind = availableDrones.RandomElement();
                spentPoints += kind.combatPower;
                defendingDrones.Add(kind);
            }
        }
    }
}