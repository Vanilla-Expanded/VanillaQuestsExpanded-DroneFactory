using Verse;
using HarmonyLib;
using System.Reflection;
namespace VanillaQuestsExpandedDroneFactory
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.VanillaQuestsExpandedDroneFactory");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
