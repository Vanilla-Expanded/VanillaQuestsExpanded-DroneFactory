using System.Linq;
using RimWorld;
using VEF.Storyteller;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class QuestPart_DroneFactory : QuestPart_Site
    {
        public string outSignalComplete;

        protected override void Enable(SignalArgs receivedArgs)
        {
            base.Enable(receivedArgs);
            CheckComplete();
        }

        public override void QuestPartTick()
        {
            base.QuestPartTick();
            if (Find.TickManager.TicksGame % 60 == 0)
                CheckComplete();
        }

        private void CheckComplete()
        {
            if (mapParent == null || !mapParent.HasMap) return;
            if (!mapParent.Map.mapPawns.AllPawnsSpawned.Any(p =>
                    !p.Dead && !p.Downed && p.Faction == mapParent.Faction))
            {
                Find.SignalManager.SendSignal(new Signal(outSignalComplete));
                Disable();
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref outSignalComplete, "outSignalComplete");
        }
    }
}