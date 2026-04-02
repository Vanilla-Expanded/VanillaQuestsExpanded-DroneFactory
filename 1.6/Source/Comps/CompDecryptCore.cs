using System.Collections.Generic;
using System;
using System.Text;
using Verse;
using Verse.AI;
using VanillaQuestsExpandedDroneFactory;
using RimWorld;

namespace VanillaQuestsExpandedDroneFactory
{



    public class CompDecryptCore : ThingComp
    {

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {

            if (selPawn != null && !StatDefOf.ResearchSpeed.Worker.IsDisabledFor(selPawn))
               
            {
                if(StudyUtility.TryFindResearchBench(selPawn, out var bench))
                {
                    if (WorldComponent_UnlockedRecipes.Instance.unlocked_Recipes.ContainsValue(false))
                    {
                        yield return new FloatMenuOption("VQE_DecryptCore".Translate(this.parent.LabelCap), () =>
                        {

                            if (selPawn.CanReserveAndReach(this.parent, PathEndMode.OnCell, Danger.Deadly))
                            {
                                Job makeJob = JobMaker.MakeJob(InternalDefOf.VQE_DecryptCore, this.parent,bench, bench?.Position ?? IntVec3.Invalid);
                                makeJob.haulMode = HaulMode.ToCellNonStorage;
                                makeJob.count = 1;
                                selPawn.jobs?.TryTakeOrderedJob(makeJob);
                            }
                        });
                    }
                    else
                    {

                        yield return new FloatMenuOption("VQE_NoUnlockableRecipes".Translate(), null);
                        yield break;
                    }

                }
                else
                {
                    yield return new FloatMenuOption("NoResearchBench".Translate().CapitalizeFirst(), null);
                    yield break;

                }


                


            }

        }


    }
}
