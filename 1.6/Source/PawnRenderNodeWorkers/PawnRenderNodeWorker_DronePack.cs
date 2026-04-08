
using Verse;
namespace VanillaQuestsExpandedDroneFactory
{
    public class PawnRenderNodeWorker_DronePack : PawnRenderNodeWorker
    {
        public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
        {
            if (base.CanDrawNow(node, parms) && !parms.Portrait && StaticCollections.pawnCapacityLabels.ContainsKey(parms.pawn.def) && parms.pawn.inventory != null)
            {
                return parms.pawn.inventory.innerContainer.Count > 0;
            }
            return false;
        }
    }
}