using RimWorld;
using System;
using UnityEngine;
using Verse;
using VEF;

namespace VanillaQuestsExpandedDroneFactory
{

    public class VanillaQuestsExpandedDroneFactory_Settings : ModSettings

    {

        public static float transmitterRadius = baseTransmitterRadius;
        public const float baseTransmitterRadius = 14.9f;

        public static float lifespanMultiplier = baseLifespanMultiplier;
        public const float baseLifespanMultiplier = 1f;


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref transmitterRadius, "transmitterRadius", baseTransmitterRadius);
            Scribe_Values.Look(ref lifespanMultiplier, "lifespanMultiplier", baseLifespanMultiplier);

        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);


            var transmitterRadiusLabel = ls.LabelPlusButton("VQE_TransmitterRadius".Translate() + ": " + transmitterRadius, "VQE_TransmitterRadiusDesc".Translate());
            transmitterRadius = (float)Math.Round(ls.Slider(transmitterRadius, 3, 50), 2);

            if (ls.Settings_Button("VQED_Reset".Translate(), new Rect(0f, transmitterRadiusLabel.position.y + 35, 250f, 29f)))
            {
                transmitterRadius = baseTransmitterRadius;
            }

            var lifespanMultiplierLabel = ls.LabelPlusButton("VQE_LifespanMultiplier".Translate() + ": x" + lifespanMultiplier, "VQE_LifespanMultiplierDesc".Translate());
            lifespanMultiplier = (float)Math.Round(ls.Slider(lifespanMultiplier, 1f, 10), 1);

            if (ls.Settings_Button("VQED_Reset".Translate(), new Rect(0f, lifespanMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                lifespanMultiplier = baseLifespanMultiplier;
            }

            ls.End();
        }





    }










}
