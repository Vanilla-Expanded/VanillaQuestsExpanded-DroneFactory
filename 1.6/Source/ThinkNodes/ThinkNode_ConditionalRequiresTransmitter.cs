using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedDroneFactory
{
    public class ThinkNode_ConditionalRequiresTransmitter : ThinkNode_Conditional
    {
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            ThinkNode_ConditionalRequiresTransmitter obj = (ThinkNode_ConditionalRequiresTransmitter)base.DeepCopy(resolve);
            return obj;
        }

        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.RequiresTransmitter();
        }
    }
}