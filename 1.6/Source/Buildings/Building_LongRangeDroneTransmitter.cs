using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Noise;
using static HarmonyLib.Code;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_LongRangeDroneTransmitter : Building, IHackable
    {

        public void OnLockedOut(Pawn pawn = null)
        {
        }

        public void OnHacked(Pawn pawn = null)
        {
            List<Pawn> allHostileDrones = Map.mapPawns.AllPawnsSpawned.Where(x => StaticCollections.pawnCapacityLabels.ContainsKey(x.def) && x.Faction != Faction.OfPlayerSilentFail).ToList();
            foreach (Pawn drone in allHostileDrones)
            {
                drone.mindState.mentalStateHandler.TryStartMentalState(InternalDefOf.VQE_Reboot, null, true);
            }
            Thing thing = ThingMaker.MakeThing(InternalDefOf.VQE_DroneCore);
            GenPlace.TryPlaceThing(thing, this.InteractionCell, Map, ThingPlaceMode.Near);


        }
    }
}
