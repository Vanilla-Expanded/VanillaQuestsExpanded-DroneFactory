using System.Collections.Generic;
using System;
using System.Text;
using Verse;
using Verse.AI;
using VanillaQuestsExpandedDroneFactory;

namespace VanillaQuestsExpandedDroneFactory
{



    public class CompDecryptCore : ThingComp
    {

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {

            if (selPawn != null)
            {

                if (WorldComponent_UnlockedRecipes.Instance.unlocked_Recipes.ContainsValue(false))
                {
                    yield return new FloatMenuOption("VQE_DecryptCore".Translate(this.parent.LabelCap), () =>
                    {

                        if (selPawn.CanReserveAndReach(this.parent, PathEndMode.OnCell, Danger.Deadly))
                        {
                            Job makeJob = JobMaker.MakeJob(InternalDefOf.VQE_DecryptCore, this.parent);
                            makeJob.haulMode = HaulMode.ToCellNonStorage;
                            makeJob.count = 1;
                            selPawn.jobs?.TryTakeOrderedJob(makeJob);


                        }

                    });

                }


            }

        }


    }
}
