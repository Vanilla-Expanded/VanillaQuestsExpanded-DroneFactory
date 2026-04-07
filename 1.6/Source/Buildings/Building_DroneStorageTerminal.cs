using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static HarmonyLib.Code;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_DroneStorageTerminal: Building,IHackable
    {

        public void OnLockedOut(Pawn pawn = null)
        {
        }

        public void OnHacked(Pawn pawn = null)
        {
            foreach (var cell in GenRadial.RadialCellsAround(Position, 14.9f, true))
            {
                if (!cell.InBounds(Map)) continue;

                List<Thing> things = Map.thingGrid.ThingsListAtFast(cell).ToList();
                for (int i = 0; i < things.Count; i++)
                {
                    if (things[i].def == InternalDefOf.VQED_DormantBattleDrone_Active)
                    {
                        Thing buildingToMake = GenSpawn.Spawn(ThingMaker.MakeThing(InternalDefOf.VQED_DormantBattleDrone), things[i].Position, Map, Rot4.North);
                        
                    }
                    if (things[i].def == InternalDefOf.VQED_DormantHornetDrone_Active)
                    {
                        Thing buildingToMake = GenSpawn.Spawn(ThingMaker.MakeThing(InternalDefOf.VQED_DormantHornetDrone), things[i].Position, Map, Rot4.North);
                       
                    }
                    if (things[i].def == InternalDefOf.VQED_DormantRaiderDrone_Active)
                    {
                        Thing buildingToMake = GenSpawn.Spawn(ThingMaker.MakeThing(InternalDefOf.VQED_DormantRaiderDrone), things[i].Position, Map, Rot4.North);
                       
                    }
                }
            }
        }
    }
}
