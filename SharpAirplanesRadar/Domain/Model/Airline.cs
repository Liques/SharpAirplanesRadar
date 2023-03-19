using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using SharpAirplanesRadar.Util;

namespace SharpAirplanesRadar
{
    /// <summary>
    /// Information about worldwide airlines
    /// </summary>
    public class Airline
    {
        private const string resourceFileName = "airlines.json";
        public string Country { get; set; }
        public string Name { get; set; }
        public string IATA { get; set; }

        static private IDictionary<string, IDictionary<string, object>> listAirlines;

        public static Airline GetAirlineByFlight(string flight)
        {
            string iata = (!String.IsNullOrEmpty(flight) && flight.Length >= 4) ? flight.Substring(0, 3) : flight;
            if (listAirlines == null)
            {
                try
                {
                    string jsonstring = ResourceHelper.LoadExternalResource(resourceFileName);

                    listAirlines = JsonConvert.DeserializeObject<IDictionary<string, IDictionary<string, object>>>(jsonstring);

                    LoggingHelper.LogBehavior($">>> Done converting {resourceFileName} file ''.");

                }
                catch (Exception e)
                {
                    throw new ArgumentException(resourceFileName, e);
                }

            }

            if (iata == null)
                iata = String.Empty;

            if (listAirlines.ContainsKey(iata))
            {
                var selectedAirline = listAirlines[iata];
                return new Airline()
                {
                    IATA = iata,
                    Country = selectedAirline["Country"].ToString(),
                    Name = selectedAirline.ContainsKey("FullName") ? selectedAirline["FullName"].ToString() : String.Empty,
                };
            }
            else
            {
                return new Airline()
                {
                    IATA = iata,
                    Country = String.Empty,
                    Name = iata,
                };

            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
