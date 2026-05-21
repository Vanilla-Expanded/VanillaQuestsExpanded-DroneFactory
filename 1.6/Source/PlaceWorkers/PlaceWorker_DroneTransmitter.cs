using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    public class PlaceWorker_DroneTransmitter : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            GenDraw.DrawFieldEdges(Utils.GetTransmitterCells(Find.CurrentMap, center));
        }
    }
}
