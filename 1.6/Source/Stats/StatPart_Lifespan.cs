
using System;
using RimWorld;
using Verse;
namespace VanillaQuestsExpandedDroneFactory
{
    public class StatPart_Lifespan : StatPart
    {


        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.HasThing)
            {
                Pawn pawn = req.Thing as Pawn;
               val *= VanillaQuestsExpandedDroneFactory_Settings.lifespanMultiplier;
                
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.HasThing)
            {
                Pawn pawn = req.Thing as Pawn;
                
                return "VQE_LifespanMultiplierModOptions".Translate(VanillaQuestsExpandedDroneFactory_Settings.lifespanMultiplier.ToStringSafe());
                
            }
            return null;
        }

       
    }
}