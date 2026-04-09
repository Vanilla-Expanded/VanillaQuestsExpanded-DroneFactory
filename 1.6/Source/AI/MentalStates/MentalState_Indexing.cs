using Verse;
using Verse.AI;
using RimWorld;
using System.Linq;

namespace VanillaQuestsExpandedDroneFactory
{
    public class MentalState_Indexing : MentalState
    {
        public Pawn target;

        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            Find.LetterStack.ReceiveLetter("VQE_IndexingLetter".Translate(pawn.Named("DRONE")), "VQE_IndexingLetterDesc".Translate(pawn.Named("DRONE")), LetterDefOf.PositiveEvent, pawn);
            FindNewTarget();
        }

        public override void PostEnd()
        {
            base.PostEnd();
            if (pawn.Spawned)
            {
                GenSpawn.Spawn(ThingMaker.MakeThing(InternalDefOf.VQE_DroneCore), pawn.Position, pawn.Map);
            }
        }

        public override void MentalStateTick(int delta)
        {
            base.MentalStateTick(delta);
            if ((target == null || !target.Spawned || !pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly)) && pawn.IsHashIntervalTick(60))
            {
                FindNewTarget();
            }
        }

        private void FindNewTarget()
        {
            var drones = pawn.Map.mapPawns.AllPawns.Where(p => p.IsDrone() && p != pawn && p.Faction == pawn.Faction && p.Spawned);
            if (drones.TryRandomElement(out var droneTarget))
            {
                target = droneTarget;
                return;
            }
            var colonists = pawn.Map.mapPawns.FreeColonistsSpawned.Where(c => c.Spawned);
            colonists.TryRandomElement(out target);
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref target, "target");
        }
    }
}
