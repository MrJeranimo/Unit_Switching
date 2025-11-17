using KSA;
using System;

namespace Unit_Switching
{

    public static class GameData
    {
        public static double CurrentOrbitalSpeed { get; private set; } = 0.0;
        public static double CurrentSurfaceSpeed { get; private set; } = 0.0;
        public static double CurrentRadarAltitude { get; private set; } = 0.0;
        public static double CurrentBarometricAltitude { get; private set; } = 0.0;
        public static Vehicle? ControlledVehicle { get; private set; } = null;
        public static Celestial? NearbyCelestial { get; private set; } = null;
        public static void UpdateData(double newOrbitalSpeed, double newSurfaceSpeed, double newRadarAltitude, double newBarometricAltitude, Vehicle? newVehicle, Celestial? newCelestial)
        {
            CurrentOrbitalSpeed = newOrbitalSpeed;
            CurrentSurfaceSpeed = newSurfaceSpeed;
            CurrentRadarAltitude = newRadarAltitude;
            CurrentBarometricAltitude = newBarometricAltitude;
            ControlledVehicle = newVehicle;
            NearbyCelestial = newCelestial;
        }
    }
}