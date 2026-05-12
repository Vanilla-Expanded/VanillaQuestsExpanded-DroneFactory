using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [HarmonyPatch(typeof(IncidentWorker_Raid), nameof(IncidentWorker_Raid.TryGenerateRaidInfo))]
    public static class IncidentWorker_Raid_TryGenerateRaidInfo_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var arriveMethod = AccessTools.Method(typeof(PawnsArrivalModeWorker), nameof(PawnsArrivalModeWorker.Arrive));
            var addDronePawnsMethod = AccessTools.Method(typeof(IncidentWorker_Raid_TryGenerateRaidInfo_Patch), nameof(AddDronePawns));
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(arriveMethod))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_2);
                    yield return new CodeInstruction(OpCodes.Ldind_Ref);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, addDronePawnsMethod);
                }
                yield return instruction;
            }
        }

        public static void AddDronePawns(List<Pawn> pawns, IncidentParms parms)
        {
            if (pawns is null || parms.faction is null) return;
            if (parms.faction.def != FactionDefOf.Pirate && parms.faction.def.replacesFaction != FactionDefOf.Pirate)
            {
                return;
            }
            if (Find.QuestManager.QuestsListForReading
                .Any(q => q.root == InternalDefOf.VQE_DroneFactory && q.State == QuestState.Ongoing) is false)
            {
                return;
            }

            var battleCount = Rand.RangeInclusive(3, 4);
            var raiderCount = Rand.RangeInclusive(1, 2);

            void AddDrone(PawnKindDef kind, int count, List<Pawn> pawns)
            {
                for (int i = 0; i < count; i++)
                {
                    var drone = PawnGenerator.GeneratePawn(kind, parms.faction);
                    pawns.Add(drone);
                }
            }
            AddDrone(InternalDefOf.VQE_BattleDrone, battleCount, pawns);
            AddDrone(InternalDefOf.VQE_RaiderDrone, raiderCount, pawns);
        }
    }
}
