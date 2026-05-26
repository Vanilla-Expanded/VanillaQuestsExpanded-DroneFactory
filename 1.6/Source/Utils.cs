using Verse;
using RimWorld;
using System.Linq;
using System.Collections.Generic;

namespace VanillaQuestsExpandedDroneFactory
{
    [HotSwappable]
    public static class Utils
    {
        public static float TransmitterRadius => VanillaQuestsExpandedDroneFactory_Settings.transmitterRadius;

        private static readonly Dictionary<int, HashSet<Building_DroneTransmitter>> transmitterCache = new Dictionary<int, HashSet<Building_DroneTransmitter>>();
        private static int lastMapId = -1;
        private static HashSet<Building_DroneTransmitter> lastMapTransmitters;

        public static void RegisterTransmitter(Building_DroneTransmitter transmitter, Map map)
        {
            if (!transmitterCache.TryGetValue(map.uniqueID, out var set))
            {
                set = new HashSet<Building_DroneTransmitter>();
                transmitterCache[map.uniqueID] = set;
            }
            set.Add(transmitter);
            lastMapId = -1;
            lastMapTransmitters = null;
        }

        public static void UnregisterTransmitter(Building_DroneTransmitter transmitter, Map map)
        {
            if (transmitterCache.TryGetValue(map.uniqueID, out var set))
            {
                set.Remove(transmitter);
            }
            lastMapId = -1;
            lastMapTransmitters = null;
        }

        public static void ClearTransmitterCache()
        {
            transmitterCache.Clear();
            lastMapId = -1;
            lastMapTransmitters = null;
        }

        public static bool IsWithinTransmitter(IntVec3 c, Map map)
        {
            int mapId = map.uniqueID;

            HashSet<Building_DroneTransmitter> set;
            if (mapId == lastMapId)
            {
                set = lastMapTransmitters;
            }
            else
            {
                transmitterCache.TryGetValue(mapId, out set);
                lastMapId = mapId;
                lastMapTransmitters = set;
            }
            if (set != null)
            {
                foreach (var t in set)
                {
                    var comp = t.TryGetComp<CompPowerTrader>();
                    if (comp == null || comp.PowerOn)
                    {
                        if (t.coveredCells.Contains(c))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsPirateOrEnemy(this Faction faction)
        {
            if (IsPirateFaction(faction.def)) return true;
            bool anyPirates = Find.FactionManager.AllFactions.Any(f => IsPirateFaction(f.def));
            if (!anyPirates)
            {
                var otherPirateFaction = Find.FactionManager.AllFactions.FirstOrDefault(f => f.HostileTo(Faction.OfPlayer) && f.def.humanlikeFaction && f.Hidden is false && f.def.defName.ToLower().Contains("pirate"));
                if (otherPirateFaction != null)
                {
                    return faction == otherPirateFaction;
                }
                return faction == Find.FactionManager.AllFactions.FirstOrDefault(f => f.HostileTo(Faction.OfPlayer) && f.def.humanlikeFaction && f.Hidden is false);
            }
            return false;
        }

        private static bool IsPirateFaction(FactionDef def)
        {
            return def == FactionDefOf.Pirate || def.replacesFaction == FactionDefOf.Pirate;
        }

        public static List<IntVec3> GetTransmitterCells(Map map, IntVec3? ghostPos = null)
        {
            var cells = new HashSet<IntVec3>();
            if (transmitterCache.TryGetValue(map.uniqueID, out var set))
            {
                foreach (var t in set)
                {
                    var comp = t.TryGetComp<CompPowerTrader>();
                    if (comp == null || comp.PowerOn)
                    {
                        cells.UnionWith(t.coveredCells);
                    }
                }
            }
            if (ghostPos.HasValue)
            {
                foreach (var cell in GenRadial.RadialCellsAround(ghostPos.Value, TransmitterRadius, true))
                {
                    if (cell.InBounds(map)) cells.Add(cell);
                }
            }
            return cells.ToList();
        }

        public static bool IsWithinTransmitter(this Pawn drone)
        {
            if (drone.RequiresTransmitter()) return IsWithinTransmitter(drone.Position, drone.Map);
            return true;
        }

        public static bool RequiresTransmitter(this Pawn pawn)
        {
            if (pawn.Faction == Faction.OfPlayer)
            {
                return pawn.GetComp<CompDrone>()?.Props.requiresTransmitter ?? false;
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
            if (!pawn.IsWithinTransmitter())
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
            if (pawn.Faction != Faction.OfPlayer) return;
            if (!CompDrone.SpawnedDrones.TryGetValue(pawn, out var comp) || !comp.Props.requiresTransmitter) return;
            if (pawn.MentalState is MentalState_SelfDecommission) return;
            bool reverse = pawn.MentalStateDef?.GetModExtension<DroneMentalStateExtension>()?.reverseNetworkRestriction ?? false;
            bool withinRange = IsWithinTransmitter(pos, pawn.Map);
            if (reverse ? withinRange : !withinRange) __result = forbidMode;
        }
    }
}
