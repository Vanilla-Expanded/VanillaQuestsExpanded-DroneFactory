using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Noise;
using static HarmonyLib.Code;
using static System.Collections.Specialized.BitVector32;

namespace VanillaQuestsExpandedDroneFactory
{
    public class Building_TinkererHoldingPlatform : Building, IHackable
    {

        public void OnLockedOut(Pawn pawn = null)
        {
        }

        public void OnHacked(Pawn pawn = null)
        {
            PawnGenerationRequest request = new PawnGenerationRequest(InternalDefOf.VQE_CraftingDroneKindDef, Faction.OfPlayerSilentFail);
            Pawn p = PawnGenerator.GeneratePawn(request);
            p.Name =PawnBioAndNameGenerator.GeneratePawnName(p, NameStyle.Full);


            GenSpawn.Spawn(p, this.InteractionCell, this.Map);


        }
    }
}
