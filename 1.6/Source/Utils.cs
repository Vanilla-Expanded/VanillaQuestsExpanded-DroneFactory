using Verse;
using RimWorld;
using System.Linq;
namespace VanillaQuestsExpandedDroneFactory
{
    [HotSwappable]
    public static class Utils
    {
        public static bool IsWithinTransmitter(IntVec3 c, Map map)
        {
            var transmitters = map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQE_DroneTransmitter);
            foreach (var t in transmitters)
            {
                if (c.DistanceTo(t.Position) <= 14.9f && t.TryGetComp<CompPowerTrader>().PowerOn) return true;
            }
            return false;
        }

        public static AcceptanceReport ShouldDisableDroneGizmos(Pawn pawn)
        {
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

        public static AcceptanceReport CanDraftDrone(Pawn pawn)
        {
            if (!IsWithinTransmitter(pawn.Position, pawn.Map))
            {
                return new AcceptanceReport("VQE_OutsideTransmitterRange".Translate());
            }
            var gizmoDisableReport = ShouldDisableDroneGizmos(pawn);
            if (!gizmoDisableReport.Accepted)
            {
                return gizmoDisableReport;
            }
            return AcceptanceReport.WasAccepted;
        }
        public static bool IsDrone(this Pawn pawn)
        {
            return pawn.GetComp<CompDrone>() != null;
        }

        public static void DestroyCore(this Pawn pawn)
        {
            var core = pawn.health.hediffSet.GetNotMissingParts().FirstOrDefault(p => p.def == InternalDefOf.VQE_DroneCoreBodyPart);
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
        public static void ApplyDroneNetworkRestriction(ref bool __result, Pawn pawn, IntVec3 pos, bool forbidMode)
        {
            bool shouldCheck = forbidMode ? __result : !__result;
            if (shouldCheck || pawn == null || pawn.Map == null) return;
            if (pawn.IsDrone())
            {
                if (pawn.MentalState is MentalState_SelfDecommission) return;
                bool reverse = false;
                if (pawn.MentalStateDef != null)
                {
                    var ext = pawn.MentalStateDef.GetModExtension<DroneMentalStateExtension>();
                    reverse = ext?.reverseNetworkRestriction ?? false;
                }
                bool withinRange = IsWithinTransmitter(pos, pawn.Map);
                bool restricted = reverse ? withinRange : !withinRange;
                if (restricted)
                {
                    __result = forbidMode;
                }
            }
        }
    }
}
