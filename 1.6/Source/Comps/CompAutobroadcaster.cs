
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace VanillaQuestsExpandedDroneFactory
{
    public enum BoadcasterMode
    {
        Work,
        Relax,
        Recruitment
    };
 


    public class CompAutobroadcaster : ThingComp
    {

        public BoadcasterMode curBroadcasterMode;

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
                        foreach (var broadcasterMode in Enum.GetValues(typeof(BoadcasterMode)).Cast<BoadcasterMode>())
                        {
                            if (broadcasterMode != curBroadcasterMode)
                            {
                                floatList.Add(new FloatMenuOption("VQE_BroadcasterMode".Translate(broadcasterMode.ToString()), delegate
                                {
                                    curBroadcasterMode = broadcasterMode;
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

