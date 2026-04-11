using HarmonyLib;
using RimWorld;
using Verse;
using PipeSystem;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(ProcessDef), nameof(ProcessDef.ShowProcess))]
    public static class VanillaQuestsExpandedDroneFactory_ProcessDef_ShowProcess_Patch
    {
        private static void Postfix(ProcessDef __instance, ref bool __result)
        {
            if (__instance.GetModExtension<ProcessDefExtension>() != null)
            {
                ProcessDefExtension extension = __instance.GetModExtension<ProcessDefExtension>();
                if (WorldComponent_UnlockedRecipes.Instance.unlocked_Recipes.ContainsKey(extension.associatedLockableRecipe))
                {
                    if (!WorldComponent_UnlockedRecipes.Instance.unlocked_Recipes[extension.associatedLockableRecipe])
                    {
                        __result = false;
                    }
                }

            }
            
        }
    }
}
