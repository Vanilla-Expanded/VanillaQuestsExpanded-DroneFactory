
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
namespace VanillaQuestsExpandedDroneFactory
{
    public class ThinkNode_ConditionalPawnKindList : ThinkNode_Conditional
    {
        public List<PawnKindDef> pawnKinds;

        public override ThinkNode DeepCopy(bool resolve = true)
        {
            ThinkNode_ConditionalPawnKindList obj = (ThinkNode_ConditionalPawnKindList)base.DeepCopy(resolve);
            obj.pawnKinds = pawnKinds;
            return obj;
        }

        protected override bool Satisfied(Pawn pawn)
        {
            return pawnKinds.Contains(pawn.kindDef);
        }
    }
}
