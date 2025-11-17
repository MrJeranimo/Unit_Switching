using HarmonyLib;
using KSA;
using System;
using System.Reflection;

namespace Unit_Switching
{

    [HarmonyPatch]
    public static class SpeedDataPatch
    {
        public static MethodBase TargetMethod()
        {

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
                Vehicle? currentVehicle = Program.ControlledVehicle;
                Celestial? nearbyCelestial = Program.GetNearbyCelestial();
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
                GameData.UpdateData(currentOrbitalSpeed, currentSurfaceSpeed, currentRadarAltitude, currentBarometricAltitude, currentVehicle, nearbyCelestial);
            }
            catch (Exception ex)
            {

            }
        }
    }
}