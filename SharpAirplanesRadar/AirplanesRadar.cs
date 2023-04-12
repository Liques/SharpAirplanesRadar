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
        internal string Token { get; }
        public bool IsCacheEnabled { get; set; }

        /// <summary>
        /// Creates a new AirplanesRadar object with a chosen API service.
        /// <param name="api">The desired API service.</param>
        /// </summary>
        public AirplanesRadar(Apis api)
        {
            var serviceProvider = Startup.Register(api);
            source = serviceProvider.GetService<IServiceAPI>();
        }

        /// <summary>
        /// Creates a new AirplanesRadar object with a chosen API service that needs a token to access the data.
        /// <param name="api">The desired API service.</param>
        /// <param name="token">Token value to access the data (Bearer Token).</param>
        /// </summary>
        public AirplanesRadar(Apis api, string token)
        {
            Token = token;
            var serviceProvider = Startup.Register(api, containsToken: true);
            source = serviceProvider.GetService<IServiceAPI>();
        }

        /// </summary>
        /// Initializes a new instance of the AirplanesRadar class with a custom API service.
        /// <param name="serviceAPI">A custom service API created externally.</param>
        /// <param name="isCacheEnabled">A boolean value that indicates whether to enable cache or not.</param>
        /// </summary>
        public AirplanesRadar(IServiceAPI serviceAPI, bool isCacheEnabled = false)
        {
            source = serviceAPI;
            this.IsCacheEnabled = isCacheEnabled;
        }

        /// <summary>
        /// Get a list of airplanes from the data provider.
        /// </summary>
        /// <param name="centerPosition">Center point (like an airport position) to load the airplanes list.</param>
        /// <param name="radiusDistanceKilometers">Radius distance in kilometers to load the data.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Set the update interval in seconds
        /// </summary>
        /// <param name="seconds">Interval in seconds</param>
        public void SetUpdateInterval(double seconds)
        {
            source.UpdateInterval = TimeSpan.FromSeconds(seconds);
        }
    }
}
