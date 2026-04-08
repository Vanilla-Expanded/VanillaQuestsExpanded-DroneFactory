using RimWorld;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld.Planet;
using VanillaQuestsExpandedDroneFactory;

namespace VanillaQuestsExpandedDroneFactory
{
    public class WorldComponent_UnlockedRecipes : WorldComponent
    {
        public static WorldComponent_UnlockedRecipes Instance;

        public Dictionary<RecipeDef, bool> unlocked_Recipes = new Dictionary<RecipeDef, bool>();

        List<RecipeDef> recipeList;
        List<bool> recipeList2;


        public WorldComponent_UnlockedRecipes(World world) : base(world)
        {
            Instance = this;
        }

        public override void FinalizeInit(bool fromLoad)
        {
            base.FinalizeInit(fromLoad);

            if (unlocked_Recipes.NullOrEmpty())
            {
                unlocked_Recipes = new Dictionary<RecipeDef, bool>() { { InternalDefOf.VQE_AssembleBattleDrone, false },
                { InternalDefOf.VQE_AssembleCleanerDrone, false },{ InternalDefOf.VQE_AssembleHaulerDrone, false },{ InternalDefOf.VQE_AssembleMinerDrone, false }
                ,{ InternalDefOf.VQE_AssembleAutobroadcasterDrone, false },{ InternalDefOf.VQE_AssembleTurretDrone, false },{ InternalDefOf.VQE_AssembleRaiderDrone, false }
                ,{ InternalDefOf.VQE_AssembleProtolancer, false },{ InternalDefOf.VQE_AssembleFarmingDrone, false },{ InternalDefOf.VQE_AssembleFirefighterDrone, false }
                ,{ InternalDefOf.VQE_AssembleHornetDrone, false },{ InternalDefOf.VQE_AssembleStingrayDrone, false },{ InternalDefOf.VQE_AssemblePackDrone, false }};

            }

        }

        public override void ExposeData()
        {
            Instance = this;
            base.ExposeData();
            Scribe_Collections.Look<RecipeDef, bool>(ref unlocked_Recipes, "unlocked_Recipes", LookMode.Def, LookMode.Value, ref recipeList, ref recipeList2);

        }


    }
}