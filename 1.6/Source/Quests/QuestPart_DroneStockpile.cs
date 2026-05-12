using System.Collections.Generic;
using System.Linq;
using RimWorld;
using VEF.Storyteller;
using Verse;
using Verse.AI.Group;

namespace VanillaQuestsExpandedDroneFactory
{
    public class QuestPart_DroneStockpile : QuestPart_Site
    {
        public List<PawnKindDef> defendingDrones = new List<PawnKindDef>();
        public string outSignalComplete;
        public Map playerMap;
        public IntRange raidIntervalTicksRange = new IntRange(5 * GenDate.TicksPerDay, 7 * GenDate.TicksPerDay);
        private int ticksUntilNextRaid;

        protected override void Enable(SignalArgs receivedArgs)
        {
            base.Enable(receivedArgs);
            ticksUntilNextRaid = raidIntervalTicksRange.RandomInRange;
        }

        public override void QuestPartTick()
        {
            base.QuestPartTick();

            if (Find.TickManager.TicksGame % 60 == 0 && Map != null && !TransmitterActiveOnMap())
            {
                Find.SignalManager.SendSignal(new Signal(outSignalComplete));
                Disable();
                return;
            }

            if (playerMap == null) return;

            ticksUntilNextRaid--;
            if (ticksUntilNextRaid <= 0)
            {
                FireRaid();
                ticksUntilNextRaid = raidIntervalTicksRange.RandomInRange;
            }
        }

        private void FireRaid()
        {
            if (playerMap == null) return;
            var faction = Faction.OfAncientsHostile;
            if (!RCellFinder.TryFindRandomPawnEntryCell(out var entryCell, playerMap, CellFinder.EdgeRoadChance_Hostile))
                return;

            var pawns = new List<Pawn>();

            void Spawn(PawnKindDef kind, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    if (!CellFinder.TryFindRandomSpawnCellForPawnNear(entryCell, playerMap, out var cell, 5))
                        continue;
                    var p = PawnGenerator.GeneratePawn(kind, faction);
                    GenSpawn.Spawn(p, cell, playerMap);
                    pawns.Add(p);
                }
            }

            Spawn(InternalDefOf.VQE_BattleDrone, Rand.RangeInclusive(3, 5));
            Spawn(InternalDefOf.VQE_HornetDrone, Rand.RangeInclusive(2, 3));
            if (Rand.RangeInclusive(0, 1) == 1)
                Spawn(InternalDefOf.VQE_RaiderDrone, 1);

            LordMaker.MakeNewLord(faction, new LordJob_AssaultColony(faction), playerMap, pawns);
            Find.LetterStack.ReceiveLetter(
                "VQE_DroneStockpileRaid".Translate(),
                "VQE_DroneStockpileRaidDesc".Translate(),
                LetterDefOf.ThreatBig,
                pawns);
        }

        private bool TransmitterActiveOnMap()
        {
            var transmitters = Map.listerThings.ThingsOfDef(InternalDefOf.VQE_LongRangeDroneTransmitter);
            if (!transmitters.Any()) return false;
            foreach (var t in transmitters)
            {
                var hackable = t.TryGetComp<CompHackable>();
                if (hackable == null || !hackable.IsHacked) return true;
            }
            return false;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref defendingDrones, "defendingDrones", LookMode.Def);
            Scribe_Values.Look(ref outSignalComplete, "outSignalComplete");
            Scribe_Values.Look(ref ticksUntilNextRaid, "ticksUntilNextRaid");
            Scribe_Values.Look(ref raidIntervalTicksRange, "raidIntervalTicksRange");
        }
    }
}
