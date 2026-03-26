using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class PawnColumnWorker_AutoRepair : PawnColumnWorker_Checkbox
    {
        protected override bool GetValue(Pawn pawn) => pawn.GetComp<CompDrone>()?.autoRepair ?? false;
        protected override void SetValue(Pawn pawn, bool value, PawnTable table)
        {
            var comp = pawn.GetComp<CompDrone>();
            if (comp != null) comp.autoRepair = value;
        }

        public override int GetOptimalWidth(PawnTable table)
        {
            return base.GetOptimalWidth(table) + 10;
        }
    }
}
