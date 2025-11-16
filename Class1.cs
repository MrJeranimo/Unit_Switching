using StarMap.API;

namespace Unit_Switching
{
    public class Unit_Switching : IStarMapMod
    {
        public bool ImmediateUnload => false;

        public void OnImmediatLoad()
        {
            
        }
        
        public void OnFullyLoaded()
        {
            Console.WriteLine("Unit_Switching Has Loaded!");
        }

        public void Unload()
        {
            
        }
    }
}
