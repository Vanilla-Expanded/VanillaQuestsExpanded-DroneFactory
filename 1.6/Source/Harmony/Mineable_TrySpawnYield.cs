using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Security.Cryptography;


namespace VanillaQuestsExpandedDroneFactory
{

    [HarmonyPatch(typeof(Mineable))]
    [HarmonyPatch("TrySpawnYield")]
    [HarmonyPatch(new Type[] { typeof(Map), typeof(bool), typeof(Pawn) })]
    public static class VanillaQuestsExpandedDroneFactory_Mineable_TrySpawnYield_Patch
    {
        [HarmonyPostfix]
        public static void ExtraYields(Map map, bool moteOnWaste, Pawn pawn, Mineable __instance)
        {
            if (pawn != null && __instance.def == InternalDefOf.VQE_CompactedDroneScrap)
            {              
                    Thing thing = ThingMaker.MakeThing(ThingDefOf.ComponentIndustrial);
                    thing.stackCount = 1;
                    GenPlace.TryPlaceThing(thing, __instance.Position, map, ThingPlaceMode.Near);

            }
        }
    }
}

