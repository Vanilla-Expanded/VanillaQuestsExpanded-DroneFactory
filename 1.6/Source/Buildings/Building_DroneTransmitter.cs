using System.Collections.Generic;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_DroneTransmitter : Building
    {
        public HashSet<IntVec3> coveredCells;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            coveredCells = new HashSet<IntVec3>();
            foreach (var cell in GenRadial.RadialCellsAround(Position, Utils.TransmitterRadius, true))
            {
                if (cell.InBounds(map))
                {
                    coveredCells.Add(cell);
                }
            }
            Utils.RegisterTransmitter(this, map);
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            var map = Map;
            base.DeSpawn(mode);
            Utils.UnregisterTransmitter(this, map);
        }
    }
}
