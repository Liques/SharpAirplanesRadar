using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SharpAirplanesRadar.APIs
{

    internal class ModeSMixer2Api : IAircraftAPI
    {
        public string GetUrl(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "")
        {
            if (String.IsNullOrEmpty(customUrl))
            {
                throw new ArgumentException("ModeSMixer requires the 'customUrl' parameter.");
            }

            return customUrl;
        }

        public IEnumerable<IAircraft> Serializer(string data)
        {
            var aircraftList = new List<IAircraft>();

            IDictionary<string, object> routes_list = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            routes_list = JsonConvert.DeserializeObject<Dictionary<string, object>>(routes_list["stats"].ToString());
            string flights = routes_list["flights"].ToString();
            var flightsJArray = JsonConvert.DeserializeObject<JArray>(routes_list["flights"].ToString()).ToList();

            for (int i = 0; i < flightsJArray.Count; i++)
            {
                var flightDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(flightsJArray[i].ToString());

                string hexcode = !flightDictionary.ContainsKey("I") ? string.Empty : flightDictionary["I"]; ;

                string flight = !flightDictionary.ContainsKey("CS") ? string.Empty : flightDictionary["CS"];
                string altitude = !flightDictionary.ContainsKey("A") ? string.Empty : flightDictionary["A"];
                string longitude = !flightDictionary.ContainsKey("LO") ? string.Empty : flightDictionary["LO"];
                string latitude = !flightDictionary.ContainsKey("LA") ? string.Empty : flightDictionary["LA"];

                string speed = !flightDictionary.ContainsKey("S") ? string.Empty : flightDictionary["S"];
                string direction = !flightDictionary.ContainsKey("D") ? string.Empty : flightDictionary["D"];
                string verticalSpeed = !flightDictionary.ContainsKey("V") ? string.Empty : flightDictionary["V"];

                string fromToPhrase = !flightDictionary.ContainsKey("FR") ? string.Empty : flightDictionary["FR"];
                string[] fromToArray = string.IsNullOrEmpty(fromToPhrase) && !fromToPhrase.Contains('-') ? null : fromToPhrase.Split('-');


                string from = fromToArray == null ? string.Empty : fromToArray[0];
                string to = fromToArray == null ? string.Empty : fromToArray.Length <= 0 ? string.Empty : fromToArray[1];
                string model = !flightDictionary.ContainsKey("ITC") ? string.Empty : flightDictionary["ITC"];
                string registration = !flightDictionary.ContainsKey("RG") ? string.Empty : flightDictionary["RG"];

                if (!string.IsNullOrEmpty(altitude))
                {
                    var newAircraft = new Airplane(
                                                        hexCode: hexcode,
                                                        flightName: flight,
                                                        altitude: AltitudeMetric.FromFoot(string.IsNullOrEmpty(altitude) ? 0 : Convert.ToDouble(altitude, CultureInfo.InvariantCulture)),
                                                        latitude: string.IsNullOrEmpty(latitude) ? 0 : Convert.ToDouble(latitude, CultureInfo.InvariantCulture),
                                                        longitude: string.IsNullOrEmpty(longitude) ? 0 : Convert.ToDouble(longitude, CultureInfo.InvariantCulture),
                                                        speed: SpeedMetric.FromKilometerPerHour(string.IsNullOrEmpty(speed) ? 0 : Convert.ToDouble(speed, CultureInfo.InvariantCulture)),
                                                        verticalSpeed: string.IsNullOrEmpty(verticalSpeed) ? 0 : Convert.ToDouble(verticalSpeed, CultureInfo.InvariantCulture),
                                                        direction: string.IsNullOrEmpty(direction) ? 0 : Convert.ToDouble(direction, CultureInfo.InvariantCulture),
                                                        registration: registration,
                                                        isOnGround: false,
                                                        from: from,
                                                        to: to,
                                                        model: model
                                                    );

                    aircraftList.Add(newAircraft);
                }
            }

            return aircraftList;
        }
    }
}
