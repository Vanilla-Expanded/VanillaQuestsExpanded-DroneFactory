using System.Linq;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MainButtonWorker_Drones : MainButtonWorker_ToggleTab
    {
        private const long DisabledCheckInterval = 180L;

        private long lastDisabledCheckTick = -10000000L;

        private int lastDisabledCheckMapId = int.MinValue;

        private bool lastDisabled;

        public override bool Disabled
        {
            get
            {
                if (base.Disabled)
                {
                    return true;
                }
                Map currentMap = Find.CurrentMap;
                int num = currentMap?.uniqueID ?? int.MinValue;
                if (GenTicks.TicksGame - lastDisabledCheckTick < 180 && lastDisabledCheckMapId == num)
                {
                    return lastDisabled;
                }
                lastDisabledCheckMapId = num;
                lastDisabledCheckTick = GenTicks.TicksGame;
                if (currentMap != null)
                {
                    if (currentMap.mapPawns.PawnsInFaction(Faction.OfPlayer).Any(p => p.IsDrone()))
                    {
                        lastDisabled = false;
                        return false;
                    }
                    if (currentMap.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQE_DroneStandby).Any())
                    {
                        lastDisabled = false;
                        return false;
                    }
                }
                lastDisabled = true;
                return true;
            }
        }

        public override bool Visible => !Disabled;
    }
}
