using Verse;
using RimWorld;
namespace VanillaQuestsExpandedDroneFactory
{
    public static class Utils
    {
        public static bool IsWithinTransmitter(IntVec3 c, Map map)
        {
            var transmitters = map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQE_DroneTransmitter);
            foreach (var t in transmitters)
            {
                if (t.TryGetComp<CompPowerTrader>().PowerOn && c.DistanceTo(t.Position) <= 14.9f) return true;
            }
            return false;
        }

        public static AcceptanceReport CanDraftDrone(Pawn pawn)
        {
            if (!IsWithinTransmitter(pawn.Position, pawn.Map))
            {
                return new AcceptanceReport("VQE_OutsideTransmitterRange".Translate());
            }
            if (pawn.MentalStateDef != null)
            {
                var ext = pawn.MentalStateDef.GetModExtension<DroneMentalStateExtension>();
                if (ext?.disableDraftGizmos ?? false)
                {
                    return new AcceptanceReport(ext.draftDisableMessage.Translate());
                }
            }
            return AcceptanceReport.WasAccepted;
        }
        public static bool IsDrone(this Pawn pawn)
        {
            return pawn.GetComp<CompDrone>() != null;
        }
    }
}
