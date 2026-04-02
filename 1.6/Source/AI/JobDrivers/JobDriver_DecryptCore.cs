using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class JobDriver_DecryptCore : JobDriver
    {
        private int useDuration = 6000;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            job.count = 1;
            this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);


            Toil toil = Toils_General.Wait(useDuration);
            toil.WithProgressBarToilDelay(TargetIndex.A);
            toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            if (job.targetA.IsValid)
            {
                toil.FailOnDespawnedOrNull(TargetIndex.A);

            }
            yield return toil;
            Toil use = ToilMaker.MakeToil();

            use.initAction = delegate
            {
                Pawn actor = use.actor;
                ThingDef core = job.targetA.Thing.def;
                Dictionary<RecipeDef, bool> allRecipes = WorldComponent_UnlockedRecipes.Instance.unlocked_Recipes;
                if (allRecipes.ContainsValue(false))
                {
                    RecipeDef recipe = allRecipes.Keys.Where(x => allRecipes[x] == false).RandomElement();
                    allRecipes[recipe] = true;

                    Find.LetterStack.ReceiveLetter("VQE_CoreDecryptedLetter".Translate(
                        recipe.products[0].thingDef.GetCompProperties<CompProperties_DroneHatcher>().hatcherKindDef.label), "VQE_CoreDecryptedDesc".Translate(
                            recipe.products[0].thingDef.GetCompProperties<CompProperties_DroneHatcher>().hatcherKindDef.label, 
                            recipe.products[0].thingDef.GetCompProperties<CompProperties_DroneHatcher>().hatcherKindDef.race.description), LetterDefOf.PositiveEvent, pawn);
                    DecreaseOrDestroy(TargetA.Thing);
                  
                }
                else
                {
                    Messages.Message("VQE_NoUnlockableRecipes".Translate(), pawn, MessageTypeDefOf.NegativeEvent, true);
                }




            };
            use.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return use;
            yield break;

        }



        public void DecreaseOrDestroy(Thing thing)
        {
            thing.stackCount--;
            if (thing.stackCount == 0) { thing.Destroy(); }
        }
    }
}
