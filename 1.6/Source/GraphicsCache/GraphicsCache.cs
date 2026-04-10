using System;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedDroneFactory
{
    [StaticConstructorOnStartup]
    public static class GraphicsCache
    {

        public static readonly Graphic graphicUnfinishedDrone = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Item/Unfinished/UnfinishedDrone", ShaderDatabase.Cutout, Vector2.one, Color.white);
       
    }
}
