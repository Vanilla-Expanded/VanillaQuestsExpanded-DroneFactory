using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using VEF.Storyteller;

namespace VanillaQuestsExpandedDroneFactory
{
    public class QuestPart_AllDronesDefeated : QuestPart_Site
    {
        public string outSignalComplete;
        public List<PawnKindDef> defendingDrones = new List<PawnKindDef>();
        protected override void Enable(SignalArgs receivedArgs)
        {
            base.Enable(receivedArgs);
            CheckComplete();
        }

        public override void QuestPartTick()
        {
            base.QuestPartTick();
            if (Find.TickManager.TicksGame % 60 == 0)
            {
                CheckComplete();
            }
        }

        private void CheckComplete()
        {
        	if (mapParent != null && mapParent.HasMap)
        	{
        		var aliveNonDownedDrones = mapParent.Map.mapPawns.PawnsInFaction(mapParent.Faction).Where(p => !p.Dead && !p.Downed && p.IsDrone());
        		if (!aliveNonDownedDrones.Any())
        		{
        			Find.SignalManager.SendSignal(new Signal(outSignalComplete));
        			Disable();
        		}
        	}
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref outSignalComplete, "outSignalComplete");
            Scribe_Collections.Look(ref defendingDrones, "defendingDrones", LookMode.Def);
        }
    }
}
