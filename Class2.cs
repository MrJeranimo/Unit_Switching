using HarmonyLib;
using KSA;
using System;
using System.Reflection;

namespace TempDebug
{

    [HarmonyPatch]
    public static class SpeedDataPatch
    {
        public static MethodBase TargetMethod()
        {
            // !!!Very Important!!!
            
            // !!!Do NOT Touch It Will Break EVERYTHING!!!

            // I don't know why though :(

            // I also don't know what it does lol

            Type vehicleType = AccessTools.TypeByName("KSA.Vehicle");

            if (vehicleType == null)
            {
                return null;
            }

            MethodInfo targetMethod = AccessTools.Method(vehicleType, "UpdateNavballData");

            return targetMethod;
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            try
            {
                // Current Vehicle you control
                Vehicle? currentVehicle = Program.ControlledVehicle;

                // Closest Celestial to your camera within 80,000km
                Celestial? nearbyCelestial = Program.GetNearbyCelestial();

                // Number of Celestials in the Universe
                int NumCelestialsRendered = Program.GetPlanetRenderer().NumCelestials;

                // The CelestialSystem that is loaded in the Universe.
                CelestialSystem? celestialSystem = KSA.Universe.CurrentSystem;

                // This thing makes no sense lol
                Dictionary<Celestial, int>.ValueCollection Celestials = Program.GetPlanetRenderer().CelestialIndices.Values;

                // Speed and Altitude data based on the controlled Vehicle
                double currentOrbitalSpeed = 0.0;
                double currentSurfaceSpeed = 0.0;
                double currentRadarAltitude = 0.0;
                double currentBarometricAltitude = 0.0;
                if (currentVehicle != null)
                {
                    currentOrbitalSpeed = currentVehicle.OrbitalSpeed;
                    currentSurfaceSpeed = currentVehicle.GetSurfaceSpeed();
                    currentRadarAltitude = currentVehicle.GetRadarAltitude();
                    currentBarometricAltitude = currentVehicle.GetBarometricAltitude();
                }

                // Update GameData every step/frame
                GameData.UpdateData(currentOrbitalSpeed, currentSurfaceSpeed, currentRadarAltitude, currentBarometricAltitude, currentVehicle, nearbyCelestial, NumCelestialsRendered, celestialSystem, Celestials);
            }
            catch (Exception ex)
            {

            }
        }
    }
}