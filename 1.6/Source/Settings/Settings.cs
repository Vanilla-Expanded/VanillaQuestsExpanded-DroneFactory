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


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref transmitterRadius, "transmitterRadius", baseTransmitterRadius);           
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

            ls.End();
        }





    }










}
