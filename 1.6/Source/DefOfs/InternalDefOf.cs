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

		[DefAlias("VQE_ShutdownDrone")]
		public static DesignationDef VQE_ShutdownDrone_Designation;
		[DefAlias("VQE_ReactivateDrone")]
		public static DesignationDef VQE_ReactivateDrone_Designation;

		public static MentalStateDef VQE_DiagnosticLoop;
		public static MentalStateDef VQE_ConnectionTimedOut;
		public static MentalStateDef VQE_SensorLag;
		public static MentalStateDef VQE_HoardingLoop;
		public static MentalStateDef VQE_Overclock;
		public static MentalStateDef VQE_Desync;
		public static MentalStateDef VQE_FluidLeak;
		public static MentalStateDef VQE_Reboot;
		public static MentalStateDef VQE_Hardcrash;
		public static MentalStateDef VQE_Killswitch;
		public static MentalStateDef VQE_SelfDecommission;
		public static MentalStateDef VQE_HardwareFailure;

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

	}
}
