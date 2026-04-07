using Verse;
using RimWorld;
namespace VanillaQuestsExpandedDroneFactory
{
	[DefOf]
	public static class InternalDefOf
	{
		static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}

		public static HediffDef VQE_Broadcaster_WorkMode;
        public static HediffDef VQE_Broadcaster_Relax;
        public static HediffDef VQE_Broadcaster_Recruitment;

		public static SoundDef VQE_AutobroadcasterSustainer_Work;
        public static SoundDef VQE_AutobroadcasterSustainer_Relax;
        public static SoundDef VQE_AutobroadcasterSustainer_Recruitment;

        public static JobDef VQE_GotoPatrolDest;
		public static NeedDef VQE_Lifespan;
		public static BodyPartDef VQE_DroneCore;
		public static JobDef VQE_RepairDrone;
		public static JobDef VQE_ShutdownDrone;
		public static JobDef VQE_ReactivateDrone;
		public static JobDef VQE_EnterStandby;
		public static JobDef VQE_StandOutOfTransmitterRange;
		public static JobDef VQE_DecryptCore;

        [DefAlias("VQE_ShutdownDrone")]
		public static DesignationDef VQE_ShutdownDrone_Designation;
		[DefAlias("VQE_ReactivateDrone")]
		public static DesignationDef VQE_ReactivateDrone_Designation;

		[DefAlias("VQE_SensorLag")]
		public static HediffDef VQE_SensorLag_Hediff;
		[DefAlias("VQE_Overclock")]
		public static HediffDef VQE_Overclock_Hediff;
		public static ThingDef VQE_MachiningFluid;
		public static ThingDef VQE_DroneTransmitter;
		public static ThingDef VQE_DroneStandby;
		public static PawnTableDef VQE_Drones;
		public static StatDef VQE_LifespanYears;
		public static StatDef VQE_LifespanDrainSpeed;
		public static PawnKindDef VQE_TurretDrone;

		public static RecipeDef VQE_AssembleBattleDrone;
		public static RecipeDef VQE_AssembleCleanerDrone;
		public static RecipeDef VQE_AssembleHaulerDrone;
		public static RecipeDef VQE_AssembleMinerDrone;
		public static RecipeDef VQE_AssembleAutobroadcasterDrone;
		public static RecipeDef VQE_AssembleTurretDrone;
		public static RecipeDef VQE_AssembleRaiderDrone;
		public static RecipeDef VQE_AssembleProtolancer;
		public static RecipeDef VQE_AssembleFarmingDrone;
		public static RecipeDef VQE_AssembleFirefighterDrone;
		public static RecipeDef VQE_AssembleHornetDrone;
		public static RecipeDef VQE_AssembleStingrayDrone;

		public static ThingDef VQE_CompactedDroneScrap;
		public static ThingDef VQED_DormantBattleDrone_Active;
		public static ThingDef VQED_DormantRaiderDrone_Active;
		public static ThingDef VQED_DormantHornetDrone_Active;
        public static ThingDef VQED_DormantBattleDrone;
        public static ThingDef VQED_DormantRaiderDrone;
        public static ThingDef VQED_DormantHornetDrone;


        public static EffecterDef VQE_SalvageableDroneWreckPulse;



    }
}
