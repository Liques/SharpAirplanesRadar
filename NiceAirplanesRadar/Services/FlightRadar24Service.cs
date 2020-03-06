using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NiceAirplanesRadar.Util;

namespace NiceAirplanesRadar.Services
{

    internal class FlightRadar24Service : ServiceAPI
    {
        private const string url = "https://data-live.flightradar24.com/zones/fcgi/feed.js?bounds=@latSouth,@latNorth,@lonWest,@lonEst&faa=1&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1";

        public FlightRadar24Service(DataLoader servicesDataLoader = null) : base(servicesDataLoader ?? new DataLoader(url), null, new TimeSpan(0, 1, 0))
        {

        }

        protected override IEnumerable<IAircraft> Conversor(string data)
        {

            var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            dataJson.Remove("full_count");
            dataJson.Remove("version");
            dataJson.Remove("stats");
            var lastAirplanesRaw = dataJson.Select(s => JsonConvert.DeserializeObject<List<string>>(s.Value.ToString())).ToList();

            var raw = lastAirplanesRaw.FirstOrDefault();

            var lastAirplanes = lastAirplanesRaw.Select(s => new Airplane(
                                                        hexCode: s[0].ToLower(),
                                                        flightName: s[16],
                                                        altitude: AltitudeMetric.FromFoot(String.IsNullOrEmpty(s[4]) ? 0 : Convert.ToDouble(s[4], CultureInfo.InvariantCulture)),
                                                        latitude: String.IsNullOrEmpty(s[1]) ? 0 : Convert.ToDouble(s[1], CultureInfo.InvariantCulture),
                                                        longitude: String.IsNullOrEmpty(s[2]) ? 0 : Convert.ToDouble(s[2], CultureInfo.InvariantCulture),
                                                        speed: SpeedMetric.FromKnot(String.IsNullOrEmpty(s[5]) ? 0 : Convert.ToDouble(s[5], CultureInfo.InvariantCulture)),
                                                        verticalSpeed: String.IsNullOrEmpty(s[6]) ? 0 : Convert.ToDouble(s[6], CultureInfo.InvariantCulture),
                                                        direction: String.IsNullOrEmpty(s[3]) ? 0 : Convert.ToDouble(s[3], CultureInfo.InvariantCulture),
                                                        registration: s[9].ToString(),
                                                        isOnGround: s[4] == "0",
                                                        from: s[11],
                                                        to: s[12],
                                                        model: s[8]
                                                    )).ToList();

            return lastAirplanes;

        }

        public override Task<IEnumerable<IAircraft>> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "")
        {
            if(centerPosition == null){
                throw new ArgumentException("FlightRadar24 requires the 'centerPosition' parameter.");
            }

            // TO DO Convert this dumb distance to real Lat/Lon distance.
            double rectangleDistance = radiusDistanceKilometers / 100;

            var newUrl = url
            .Replace("@latNorth", (centerPosition.Latitude - rectangleDistance).ToString(CultureInfo.InvariantCulture))
            .Replace("@latSouth", (centerPosition.Latitude + rectangleDistance).ToString(CultureInfo.InvariantCulture))
            .Replace("@lonWest", (centerPosition.Longitude - rectangleDistance).ToString(CultureInfo.InvariantCulture))
            .Replace("@lonEst", (centerPosition.Longitude + rectangleDistance).ToString(CultureInfo.InvariantCulture));

            return base.GetAirplanes(
                centerPosition: centerPosition,
                radiusDistanceKilometers: 0,
                cacheEnabled: cacheEnabled,
                customUrl: newUrl);

        }

    }
}
