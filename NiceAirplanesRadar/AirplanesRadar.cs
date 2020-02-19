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

        public AirplanesRadar(SourceAPI sourceTypeEnum, bool isCacheEnabled = true)
        {
            Type serviceType = null;
            switch (sourceTypeEnum)
            {
                case SourceAPI.OpenSky:
                    serviceType = typeof(OpenSkyService);
                    break;
                case SourceAPI.FlightRadar24:
                    serviceType = typeof(FlightRadar24Service);
                    break;
                case SourceAPI.ModeSMixer2:
                    serviceType = typeof(ModeSMixer2Service);
                    break;
                default:
                    throw new NotSupportedException();
            }

            source = (ServiceAPI)Activator.CreateInstance(serviceType);
            this.IsCacheEnabled = isCacheEnabled;

            LoggingHelper.LogBehavior("> INIT basic data...");
            var fooAirplane = new Aircraft("0", "0", 0, 0, 0, 0, 0, 0, "", "", "A319", "0", false);
            LoggingHelper.LogBehavior("> DONE basic data.");
        }

        public IEnumerable<Aircraft> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100)
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
