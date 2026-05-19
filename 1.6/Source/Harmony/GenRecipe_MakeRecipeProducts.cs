using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;


namespace VanillaQuestsExpandedDroneFactory
{

    [HarmonyPatch(typeof(GenRecipe))]
    [HarmonyPatch("MakeRecipeProducts")]

    public static class VanillaQuestsExpandedDroneFactory_GenRecipe_MakeRecipeProducts_Patch
    {
        [HarmonyPostfix]
        public static void Dissassemble(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients)
        {
            if (recipeDef == InternalDefOf.VQE_DisassembleDrone)
            {
                Corpse corpse = ingredients[0] as Corpse;

                CompDrone compDrone = corpse.InnerPawn.TryGetComp<CompDrone>();

                List<ThingDefCountClass> possibleOutputs= new List<ThingDefCountClass>();
                if (compDrone != null && compDrone.isPlayerDrone) {
                    possibleOutputs = corpse.InnerPawn.def.costList;                  
                }
                else
                {
                    possibleOutputs = corpse.InnerPawn.def.butcherProducts;                    
                }
                foreach (ThingDefCountClass item in possibleOutputs)
                {
                    Thing thing = ThingMaker.MakeThing(item.thingDef, item.stuff);
                    thing.stackCount = item.count;
                    if (!GenPlace.TryPlaceThing(thing, worker.Position, worker.Map, ThingPlaceMode.Near))
                    {
                        Log.Error(worker?.ToString() + " could not drop recipe product " + thing?.ToString() + " near " + worker.Position.ToString());
                    }

                }




            }
        }
    }
}

