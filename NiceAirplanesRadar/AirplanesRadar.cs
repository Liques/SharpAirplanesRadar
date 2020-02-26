using System;
using System.Collections.Generic;
using NiceAirplanesRadar.Services;
using NiceAirplanesRadar.Util;

namespace NiceAirplanesRadar
{
    public class AirplanesRadar
    {
        private ServiceAPI source;
        public static bool DebugMode { get { return LoggingHelper.ShowBehaviorLog; } set { LoggingHelper.ShowBehaviorLog = value; } }
        public bool IsCacheEnabled { get; set; }

        public AirplanesRadar(SourceAPI sourceTypeEnum, bool isCacheEnabled = false)
        {
            source = (ServiceAPI)Activator.CreateInstance(Type.GetType($"{typeof(ServiceAPI).Namespace}.{Enum.GetName(typeof(SourceAPI),sourceTypeEnum)}Service"));
            this.IsCacheEnabled = isCacheEnabled;

            LoggingHelper.LogBehavior("> INIT basic data...");
            var fooAirplane = new Airplane("0", "0", AltitudeMetric.FromMeter(0), 0, 0, SpeedMetric.FromKnot(0), 0, 0, "", "", "A319", "0", false);
            LoggingHelper.LogBehavior("> DONE basic data.");
        }

        public IEnumerable<IAircraft> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100)
        {
            return source.GetAirplanes(centerPosition, radiusDistanceKilometers, this.IsCacheEnabled);
        }

        /// <summary>
        /// Load an external database file with data from the most airplanes of the world.
        /// Currently it is only supporting the CSV file provided by Open Sky Network (https://opensky-network.org/aircraft-database).
        /// </summary>
        public static void LoadAircraftDatabase(string fileName, AircraftDatabaseType database)
        {
            AircraftDatabase.LoadAircraftDatabaseFromOpenSky(fileName);
        }

    }
}
