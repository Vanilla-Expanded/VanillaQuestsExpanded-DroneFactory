using RimWorld;
using Verse;
using Verse.Sound;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_DroneTrap : Building_Trap
    {

        public bool signalSprung = false;
        public int deletionCounter = 0;
      
        DroneTrapDetails cachedExtension;

        public DroneTrapDetails CachedExtension
        {
            get
            {
                if (cachedExtension is null)
                {
                    cachedExtension = this.def.GetModExtension<DroneTrapDetails>();
                }
                return cachedExtension;
            }
        }


        protected override void Tick()
        {

            if (signalSprung)
            {
                deletionCounter++;
               
                if (deletionCounter > 30)
                {
                    IntVec3 pos = this.PositionHeld;
                    Map map = this.Map;

                    PopUpMonster(pos, map);

                    if (this.Spawned)
                    {
                        this.Destroy();
                    }
                }
            }
            if (!signalSprung && Spawned && this.IsHashIntervalTick(10))
            {
                int numCells = GenRadial.NumCellsInRadius(6);
                for (int i = 0; i < numCells; i++)
                {
                    IntVec3 intVec = this.Position + GenRadial.RadialPattern[i];
                    if (intVec.InBounds(this.Map))
                    {
                        foreach (Thing thing in intVec.GetThingList(this.Map))
                        {
                            if (thing != null && thing is Pawn detectedPawn && detectedPawn.RaceProps.Humanlike && detectedPawn.GetRoom() == this.GetRoom())
                            {
                                this.SpringSub(detectedPawn);
                            }
                        }
                    }
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.signalSprung, "signalSprung");

        }

        public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostApplyDamage(dinfo, totalDamageDealt);
            this.SpringSub(null);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            this.SpringSub(null);
            base.Destroy(mode);
        }


        protected override void SpringSub(Pawn p)
        {
      
            signalSprung = true;

        }

        public void PopUpMonster(IntVec3 pos, Map map)
        {
            if (CachedExtension.deconstructSound != null)
            {
                CachedExtension.deconstructSound.PlayOneShot(this);
            }

            Pawn pawn = PawnGenerator.GeneratePawn(CachedExtension.droneSpawn, Faction.OfAncientsHostile);
            GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(pos, map, 1), map);
          

        }

    }
}
