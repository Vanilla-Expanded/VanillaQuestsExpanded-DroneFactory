using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MainTabWindow_Drones : MainTabWindow_PawnTable
    {
        protected override PawnTableDef PawnTableDef => InternalDefOf.VQE_Drones;
        protected override IEnumerable<Pawn> Pawns
        {
            get
            {
                var spawnedDrones = Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer).Where(p => p.IsDrone());
                var standbyDrones = Find.CurrentMap.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQE_DroneStandby).OfType<Building_DroneStandby>().Select(b => b.drone);
                return spawnedDrones.Concat(standbyDrones);
            }
        }
    }
}
