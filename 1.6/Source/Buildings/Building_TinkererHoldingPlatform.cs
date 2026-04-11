using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Noise;
using static HarmonyLib.Code;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_TinkererHoldingPlatform : Building, IHackable
    {

        public void OnLockedOut(Pawn pawn = null)
        {
        }

        public void OnHacked(Pawn pawn = null)
        {
            Pawn p = PawnGenerator.GeneratePawn(InternalDefOf.VQE_CraftingDroneKindDef, Faction.OfPlayerSilentFail);
            GenSpawn.Spawn(p, this.InteractionCell, this.Map);


        }
    }
}
