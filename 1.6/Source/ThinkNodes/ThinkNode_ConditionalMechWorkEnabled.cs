
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
namespace VanillaQuestsExpandedDroneFactory
{
    public class ThinkNode_ConditionalMechWorkEnabled : ThinkNode_Conditional
    {
        public List<WorkTypeDef> workTypes;

        protected override bool Satisfied(Pawn pawn)
        {
            return !pawn.RaceProps.mechEnabledWorkTypes.NullOrEmpty() && !pawn.RaceProps.mechEnabledWorkTypes.Except(workTypes).Any();
        }
    }
}
