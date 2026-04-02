using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(RecipeDef), nameof(RecipeDef.AvailableNow),MethodType.Getter)]
    public static class VanillaQuestsExpandedDroneFactory_RecipeDef_AvailableNow_Patch
    {
        private static void Postfix(RecipeDef __instance, ref bool __result)
        {
            if (WorldComponent_UnlockedRecipes.Instance.unlocked_Recipes.ContainsKey(__instance))
            {
                if (!WorldComponent_UnlockedRecipes.Instance.unlocked_Recipes[__instance])
                {
                    __result = false;
                }
            }
        }
    }
}
