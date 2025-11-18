using Brutal.ImGuiApi;
using HarmonyLib;
using KSA;
using StarMap.API;
using System.Diagnostics;
using System.Reflection;

namespace TempDebug
{
    [StarMapMod]
    public class Unit_Switching
    {
        private const string HarmonyId = "com.extendicator.starmap";
        private readonly Harmony _harmony = new Harmony(HarmonyId);
        private bool _showWindow = true;
        private bool _showSettings = false;
        private bool _uiDrawnThisFrame = false;
        private double _lastKnownOrbitalSpeed = 0.0;
        private double _lastKnownSurfaceSpeed = 0.0;
        private double _lastKnownRadarAltitude = 0.0;
        private double _lastKnownBarometricAltitude = 0.0;
        private Vehicle? _controlledVehicle = null;
        private Celestial? _nearbyCelestial = null;
        private int _numCelestialsRendered = 0;
        private CelestialSystem? _celestialSystem = null;
        private Dictionary<Celestial, int>.ValueCollection? _celestials = null;
        private List<Astronomical>? _astronomicals = null;

        private static string CorrectDistanceUnits(double distance)
        {
            // Creates a string with the correct units based on distance
            // Units {m, Km, Mm}
            if (distance < 10000.0) 
            {
                return $"{distance:F0}m";
            } 
            else if (distance > 1000000000.0)
            {
                return $"{distance/1000000.0:F3}Mm";
            }
            else
            {
                return $"{distance/1000.0:F3}Km";
            }
        }

        [StarMapAllModsLoaded]
        public void OnFullyLoaded()
        {
            // No Clue Either, Just Works.
            try
            {
                _harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {

            }
        }

        [StarMapUnload]
        public void Unload()
        {
            // Similar to Above
            _harmony.UnpatchAll(HarmonyId);
        }

        [StarMapBeforeGui]
        public void OnBeforeUi(double dt)
        {
            _uiDrawnThisFrame = false;
        }

        [StarMapAfterGui]
        public void OnAfterUi(double dt)
        {
            if (_uiDrawnThisFrame) return;

            DrawUi();
            _uiDrawnThisFrame = true;
        }

        private void DrawUi()
        {
            if (!_showWindow) return;

            // Update relevant data each UI frame
            _lastKnownOrbitalSpeed = GameData.CurrentOrbitalSpeed;
            _lastKnownSurfaceSpeed = GameData.CurrentSurfaceSpeed;
            _lastKnownRadarAltitude = GameData.CurrentRadarAltitude;
            _lastKnownBarometricAltitude = GameData.CurrentBarometricAltitude;
            _controlledVehicle = GameData.ControlledVehicle;
            _nearbyCelestial = GameData.NearbyCelestial;
            _numCelestialsRendered = GameData.NumCelestialsRendered;
            _celestialSystem = GameData.curCelestialSystem;
            _celestials = GameData.allCelestials;

            if (_celestialSystem != null)
            {
                // Gets a list of astronomicals in the system
                // This includes Vehicles
                _astronomicals = _celestialSystem.All.GetList();
            }

            ImGuiWindowFlags flags = ImGuiWindowFlags.None;
            ImGui.Begin("TempDebug", ref _showWindow, flags);

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.MenuItem("Settings"))
                    _showSettings = !_showSettings;
                ImGui.EndMenuBar();
            }
            
            ImGui.TextWrapped("Original mod made by BarneyTheGod. Extended by MrJeranimo");

            ImGui.Separator();
            if(_controlledVehicle != null)
            {
                ImGui.Text($"Current Vehicle: {_controlledVehicle.Id}");
            }
            if (_nearbyCelestial != null)
            {
                ImGui.Text($"Nearest Celestial: {_nearbyCelestial.Id}");
            }
            ImGui.Text($"Number of Celestials in Universe: {_numCelestialsRendered}");
            ImGui.Separator();
            ImGui.Text($"Orbital Speed: {_lastKnownOrbitalSpeed:F2}m/s");
            ImGui.Text($"Surface Speed: {_lastKnownSurfaceSpeed:F2}m/s");
            ImGui.Text($"Speed Difference (O-S): {_lastKnownOrbitalSpeed-_lastKnownSurfaceSpeed:F2}m/s");
            ImGui.Separator();
            String correctRadarA = CorrectDistanceUnits(_lastKnownRadarAltitude);
            ImGui.Text($"Radar Altitude (Ground): {correctRadarA}");
            String correctBarometricA = CorrectDistanceUnits(_lastKnownBarometricAltitude);
            ImGui.Text($"Barometric Altitude (Sea Level): {correctBarometricA}");
            String correctDifferenceA = CorrectDistanceUnits(_lastKnownBarometricAltitude-_lastKnownRadarAltitude);
            ImGui.Text($"Altitude Difference (B-R): {correctDifferenceA}");

            if(_astronomicals != null)
            {
                foreach(var astro in _astronomicals)
                {
                    // Prints out each Astronomicals ID (Name)
                    ImGui.Text($"Astronomical: {astro.Id}");
                }
            } 
            else
            {
                Console.WriteLine("Astronomicals is null");
            }

            if (_celestials != null)
            {
                foreach (var cel in _celestials)
                {
                    ImGui.Text($"Celestial: {cel}");
                }
            }
            else
            {
                Console.WriteLine("Celestials is null");
            }




            if (_showSettings)
            {
                ImGui.Separator();
                ImGui.Text("Soon.");
            }

            ImGui.End();
        }

        [StarMapImmediateLoad]
        public void OnImmediatLoad()
        {
            Console.WriteLine("TempDebug loaded!");
        }
    }
}
