
using RimWorld;
using Verse;
namespace VanillaQuestsExpandedDroneFactory
{
    public class StatPart_DroneWorkspeedStatOffset : StatPart
    {

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (TryGetOffset(req, out float offset))
            {
                val += offset;
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (TryGetOffset(req, out float offset) && offset != 0f)
            {
                return "VQE_DroneWorkspeedStatOffset" + ": +" + offset.ToStringPercent();
            }
            return null;
        }

        private bool TryGetOffset(StatRequest req, out float offset)
        {
            if (ModsConfig.BiotechActive && ModsConfig.IdeologyActive && req.HasThing && Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.MechanoidLabor_Enhanced) != null)
            {
                Pawn pawn = req.Thing as Pawn;
                if (pawn != null && StaticCollections.pawnCapacityLabels.ContainsKey(pawn.def))
                {
                    offset = 0.2f;
                    return true;
                }
            }
            offset = 0f;
            return false;
        }
    }
}