using Verse;
using RimWorld;
using System.Linq;
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

        public static void DestroyCore(this Pawn pawn)
        {
            var core = pawn.health.hediffSet.GetNotMissingParts().FirstOrDefault(p => p.def == InternalDefOf.VQE_DroneCore);
            if (core != null)
            {
                var damage = new DamageInfo(DamageDefOf.Crush, 999f, 999f, -1f, null, core);
                damage.SetAllowDamagePropagation(false);
                pawn.TakeDamage(damage);
            }
            if (pawn.Dead is false)
                pawn.Kill(null);
            MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
        }
    }
}
