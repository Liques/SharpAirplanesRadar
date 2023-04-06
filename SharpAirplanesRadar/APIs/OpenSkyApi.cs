using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SharpAirplanesRadar.Util;
using SharpAirplanesRadar.Services;

namespace SharpAirplanesRadar.APIs
{
    internal class OpenSkyApi : IRadarAPI
    {
        private const string url = "https://opensky-network.org/api/states/all";

        public string GetUrl(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "")
        {
            return url;
        }

        public IEnumerable<IAircraft> Serializer(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Enumerable.Empty<IAircraft>();
            }

            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

            if (!jsonData.ContainsKey("states"))
            {
                return Enumerable.Empty<IAircraft>();
            }

            var lastAirplanesRaw = JsonConvert.DeserializeObject<List<string[]>>(jsonData["states"].ToString());

            var raw = lastAirplanesRaw.FirstOrDefault();

            var lastAirplanes = lastAirplanesRaw.Select(s => new Airplane(
                                                        hexCode: s[0],
                                                        flightName: s[1], // flightname
                                                        altitude: AltitudeMetric.FromMeter(string.IsNullOrEmpty(s[7]) ? 0 : Convert.ToDouble(s[7], CultureInfo.InvariantCulture)),
                                                        latitude: string.IsNullOrEmpty(s[6]) ? 0 : Convert.ToDouble(s[6], CultureInfo.InvariantCulture),
                                                        longitude: string.IsNullOrEmpty(s[5]) ? 0 : Convert.ToDouble(s[5], CultureInfo.InvariantCulture),
                                                        speed: SpeedMetric.FromKilometerPerHour(string.IsNullOrEmpty(s[9]) ? 0 : Convert.ToDouble(s[9], CultureInfo.InvariantCulture) * 3.6),
                                                        verticalSpeed: string.IsNullOrEmpty(s[11]) ? 0 : Convert.ToDouble(s[11], CultureInfo.InvariantCulture),
                                                        direction: string.IsNullOrEmpty(s[10]) ? 0 : Convert.ToDouble(s[10], CultureInfo.InvariantCulture),
                                                        registration: s[2],
                                                        isOnGround: bool.Parse(s[8]),
                                                        from: string.Empty,
                                                        to: string.Empty,
                                                        model: string.Empty
                                                    )).ToList();

            return lastAirplanes;
        }
    }
}
