
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Verse.Sound;

namespace VanillaQuestsExpandedDroneFactory
{
    public enum BroadcasterMode
    {
        Work,
        Relax,
        Recruitment
    };

    public class CompAutobroadcaster : ThingComp
    {

        public BroadcasterMode curBroadcasterMode;
        private Sustainer sustainer;

        public CompProperties_Autobroadcaster Props
        {
            get
            {
                return (CompProperties_Autobroadcaster)this.props;
            }
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref curBroadcasterMode, "curBroadcasterMode");
           
        }

        public void RemoveHediffs(Pawn pawn)
        {
            Hediff workHediff = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VQE_Broadcaster_WorkMode);
            Hediff relaxHediff = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VQE_Broadcaster_Relax);
            Hediff recruitmentHediff = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VQE_Broadcaster_Recruitment);
            if (workHediff != null) { pawn.health.RemoveHediff(workHediff); }
            if (relaxHediff != null) { pawn.health.RemoveHediff(relaxHediff); }
            if (recruitmentHediff != null) { pawn.health.RemoveHediff(recruitmentHediff); }
        }

        public void InitializeSustainer(SoundDef sound)
        {
            if (sustainer == null || sustainer.Ended)
            {
                sustainer = sound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
            }
        }

        public void EndSustainer()
        {
            if (sustainer!=null && !sustainer.Ended)
            {
                sustainer?.End();
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (sustainer != null)
            {
                sustainer.Maintain();
            }
            else
            {
                InitializeSustainer(SustainerFromMode(curBroadcasterMode));
            }
        }

        public SoundDef SustainerFromMode(BroadcasterMode mode)
        {
            switch (mode)
            {
                case BroadcasterMode.Work:                  
                    return InternalDefOf.VQE_AutobroadcasterSustainer_Work;                  
                case BroadcasterMode.Relax:
                    return InternalDefOf.VQE_AutobroadcasterSustainer_Relax;               
                case BroadcasterMode.Recruitment:
                    return InternalDefOf.VQE_AutobroadcasterSustainer_Recruitment;                 
            }
            return null;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }
            if (this.parent.Faction == Faction.OfPlayer)
            {
                yield return new Command_Action
                {
                    defaultLabel = "VQE_BroadcasterMode".Translate(curBroadcasterMode.ToString()),
                    defaultDesc = ("VQE_BroadcasterModeDesc_" + curBroadcasterMode.ToString()).Translate(),
                    action = () =>
                    {
                        var floatList = new List<FloatMenuOption>();
                        foreach (var broadcasterMode in Enum.GetValues(typeof(BroadcasterMode)).Cast<BroadcasterMode>())
                        {
                            if (broadcasterMode != curBroadcasterMode)
                            {
                                floatList.Add(new FloatMenuOption("VQE_BroadcasterMode".Translate(broadcasterMode.ToString()), delegate
                                {
                                    curBroadcasterMode = broadcasterMode;
                                    EndSustainer();
                                    Pawn pawn = parent as Pawn;
                                    RemoveHediffs(pawn);
                                    switch (curBroadcasterMode)
                                    {
                                        case BroadcasterMode.Work:
                                            pawn.health.AddHediff(InternalDefOf.VQE_Broadcaster_WorkMode);
                                            InitializeSustainer(InternalDefOf.VQE_AutobroadcasterSustainer_Work);
                                            break;
                                        case BroadcasterMode.Relax:
                                            pawn.health.AddHediff(InternalDefOf.VQE_Broadcaster_Relax);
                                            InitializeSustainer(InternalDefOf.VQE_AutobroadcasterSustainer_Relax);
                                            break;
                                        case BroadcasterMode.Recruitment:
                                            pawn.health.AddHediff(InternalDefOf.VQE_Broadcaster_Recruitment);
                                            InitializeSustainer(InternalDefOf.VQE_AutobroadcasterSustainer_Recruitment);
                                            break;
                                    }
                                    
                                }));
                            }
                        }
                        Find.WindowStack.Add(new FloatMenu(floatList));
                    },
                    icon = ContentFinder<Texture2D>.Get("UI/Gizmos/AutobroadcasterMode_" + curBroadcasterMode.ToString())
                };
               
            }
        }
    }
}

