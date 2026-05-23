using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_DroneTransmitter : Building
    {
        public static readonly Dictionary<int, Dictionary<IntVec3, int>> CoveredCells = new();

        private static Dictionary<IntVec3, int> GetOrCreateCells(int mapId)
        {
            if (!CoveredCells.TryGetValue(mapId, out var cells))
                CoveredCells[mapId] = cells = new Dictionary<IntVec3, int>();
            return cells;
        }

        public static bool IsWithinTransmitter(IntVec3 c, int mapId)
        {
            return CoveredCells.TryGetValue(mapId, out var cells) && cells.ContainsKey(c);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            AddCells();
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            RemoveCells();
            base.DeSpawn(mode);
        }

        protected override void ReceiveCompSignal(string signal)
        {
            base.ReceiveCompSignal(signal);
            if (signal == "PowerTurnedOn") AddCells();
            else if (signal == "PowerTurnedOff") RemoveCells();
        }

        private void AddCells()
        {
            if (!this.TryGetComp<CompPowerTrader>().PowerOn) return;
            var cells = GetOrCreateCells(Map.uniqueID);
            foreach (var cell in GenRadial.RadialCellsAround(Position, Utils.TransmitterRadius, true))
            {
                if (!cell.InBounds(Map)) continue;
                cells.TryGetValue(cell, out var count);
                cells[cell] = count + 1;
            }
        }

        private void RemoveCells()
        {
            if (Map == null) return;
            var cells = GetOrCreateCells(Map.uniqueID);
            foreach (var cell in GenRadial.RadialCellsAround(Position, Utils.TransmitterRadius, true))
            {
                if (!cell.InBounds(Map)) continue;
                if (!cells.TryGetValue(cell, out var count)) continue;
                if (count <= 1) cells.Remove(cell);
                else cells[cell] = count - 1;
            }
        }
    }
}
