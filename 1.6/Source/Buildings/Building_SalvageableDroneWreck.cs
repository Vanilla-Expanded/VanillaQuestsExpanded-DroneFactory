
using RimWorld;
using RimWorld.Planet;
using Verse;
namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_SalvageableDroneWreck : Building
    {
        private Effecter effecter;

        protected override void Tick()
        {
            base.Tick();
            if (effecter == null)
            {
                effecter = InternalDefOf.VQE_SalvageableDroneWreckPulse.SpawnAttached(this, Map);
            }
            effecter?.EffectTick(this, this);
        }

    }
}