using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
	public class Dialog_RenameDrone : Dialog_Rename
	{
		public Pawn pawn;

		public Dialog_RenameDrone(Pawn pawn)
		{
			this.pawn = pawn;
			curName = pawn.LabelCap;
		}

		protected override void SetName(string name)
		{
			pawn.Name = new NameSingle(name);
		}
	}
}
