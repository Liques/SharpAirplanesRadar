using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using NiceAirplanesRadar.Util;

namespace NiceAirplanesRadar.Services
{

    internal class ModeSMixer2Service : ServiceAPI
    {
        public ModeSMixer2Service(DataLoader servicesDataLoader = null) : base(servicesDataLoader ?? new DataLoader(null), null, new TimeSpan(0, 0, 15))
        {

        }

        protected override IEnumerable<IAircraft> Conversor(string data)
        {
            var aircraftList = new List<IAircraft>();

            IDictionary<string, object> routes_list = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            routes_list = JsonConvert.DeserializeObject<Dictionary<string, object>>(routes_list["stats"].ToString());
            string flights = routes_list["flights"].ToString();
            var flightsJArray = JsonConvert.DeserializeObject<JArray>(routes_list["flights"].ToString()).ToList();

            for (int i = 0; i < flightsJArray.Count; i++)
            {
                var flightDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(flightsJArray[i].ToString());

                string hexcode = !flightDictionary.ContainsKey("I") ? String.Empty : flightDictionary["I"]; ;

                string flight = !flightDictionary.ContainsKey("CS") ? String.Empty : flightDictionary["CS"];
                string altitude = !flightDictionary.ContainsKey("A") ? String.Empty : flightDictionary["A"];
                string longitude = !flightDictionary.ContainsKey("LO") ? String.Empty : flightDictionary["LO"];
                string latitude = !flightDictionary.ContainsKey("LA") ? String.Empty : flightDictionary["LA"];

                string speed = !flightDictionary.ContainsKey("S") ? String.Empty : flightDictionary["S"];
                string direction = !flightDictionary.ContainsKey("D") ? String.Empty : flightDictionary["D"];
                string verticalSpeed = !flightDictionary.ContainsKey("V") ? String.Empty : flightDictionary["V"];

                string fromToPhrase = !flightDictionary.ContainsKey("FR") ? String.Empty : flightDictionary["FR"];
                string[] fromToArray = String.IsNullOrEmpty(fromToPhrase) && !fromToPhrase.Contains('-') ? null : fromToPhrase.Split('-');


                string from = fromToArray == null ? String.Empty : fromToArray[0];
                string to = fromToArray == null ? String.Empty : fromToArray.Length <= 0 ? String.Empty : fromToArray[1];
                string model = !flightDictionary.ContainsKey("ITC") ? String.Empty : flightDictionary["ITC"];
                string registration = !flightDictionary.ContainsKey("RG") ? String.Empty : flightDictionary["RG"];

                if (!String.IsNullOrEmpty(altitude))
                {
                    var newAircraft = new Airplane(
                                                        hexCode: hexcode,
                                                        flightName: flight,
                                                        altitude: AltitudeMetric.FromFoot(String.IsNullOrEmpty(altitude) ? 0 : Convert.ToDouble(altitude, CultureInfo.InvariantCulture)),
                                                        latitude: String.IsNullOrEmpty(latitude) ? 0 : Convert.ToDouble(latitude, CultureInfo.InvariantCulture),
                                                        longitude: String.IsNullOrEmpty(longitude) ? 0 : Convert.ToDouble(longitude, CultureInfo.InvariantCulture),
                                                        speed: SpeedMetric.FromKilometerPerHour(String.IsNullOrEmpty(speed) ? 0 : Convert.ToDouble(speed, CultureInfo.InvariantCulture)),
                                                        verticalSpeed: String.IsNullOrEmpty(verticalSpeed) ? 0 : Convert.ToDouble(verticalSpeed, CultureInfo.InvariantCulture),
                                                        direction: String.IsNullOrEmpty(direction) ? 0 : Convert.ToDouble(direction, CultureInfo.InvariantCulture),
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

        public override Task<IEnumerable<IAircraft>> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "")
        {
            if (String.IsNullOrEmpty(customUrl))
            {
                throw new ArgumentException("ModeSMixer requires the 'customUrl' parameter.");
            }

            return base.GetAirplanes(
                centerPosition: centerPosition,
                radiusDistanceKilometers: radiusDistanceKilometers,
                cacheEnabled: cacheEnabled,
                customUrl: customUrl);

        }

    }
}
