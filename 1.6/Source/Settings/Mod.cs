using RimWorld;
using UnityEngine;
using Verse;


namespace VanillaQuestsExpandedDroneFactory
{

    public class VanillaQuestsExpandedDroneFactory_Mod : Mod
    {


        public VanillaQuestsExpandedDroneFactory_Mod(ModContentPack content) : base(content)
        {
            GetSettings<VanillaQuestsExpandedDroneFactory_Settings>();
        }

        public override string SettingsCategory()
        {
            return "VQE - Drone Factory";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            VanillaQuestsExpandedDroneFactory_Settings.DoWindowContents(inRect);
        }
    }


}
