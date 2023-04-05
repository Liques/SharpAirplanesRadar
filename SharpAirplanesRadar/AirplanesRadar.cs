using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SharpAirplanesRadar.Domain.Enum;
using SharpAirplanesRadar.Util;

namespace SharpAirplanesRadar
{
    public class AirplanesRadar
    {
        private IServiceAPI source;
        public static bool DebugMode { get { return LoggingHelper.ShowBehaviorLog; } set { LoggingHelper.ShowBehaviorLog = value; } }
        public bool IsCacheEnabled { get; set; }

        public AirplanesRadar(Apis api)
        {
            var serviceProvider = Startup.Register(api);
            source = serviceProvider.GetService<IServiceAPI>();
        }

        public AirplanesRadar(IServiceAPI serviceAPI, bool isCacheEnabled = false)
        {
            source = serviceAPI;
            this.IsCacheEnabled = isCacheEnabled;
        }

        public async Task<IEnumerable<IAircraft>> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100)
        {
            return await source.GetAirplanes(centerPosition, radiusDistanceKilometers, this.IsCacheEnabled);
        }

        /// <summary>
        /// Load an external database file with data from the most airplanes of the world.
        /// Currently it is only supporting the CSV file provided by Open Sky Network (https://opensky-network.org/aircraft-database).
        /// </summary>
        public static void LoadAircraftDatabase(string fileName, AircraftDatabaseType database)
        {
            AircraftDatabase.LoadAircraftDatabaseFromOpenSky(fileName);
        }

        public void SetUpdateInterval(int seconds)
        {
            source.UpdateInterval = new TimeSpan(0,0,seconds);
        }
    }
}
