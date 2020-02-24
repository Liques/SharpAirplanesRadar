﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NiceAirplanesRadar.Util;
using NiceAirplanesRadar;

namespace NiceAirplanesRadar
{
    public class Airport
    {
        private const string resourceFileName = "airports.json";
        public string City { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public string IATA { get; set; }
        public string ICAO { get; set; }
        public GeoPosition Position { get; set; }
        public List<Runway> ListRunways { get; set; }
        public bool IsValid { get; set; }
        public AltitudeMetric Altitude { get; set; }
        public string GeoName { get; set; }
        public string GeoNameState { get; set; }
        public string GeoCountry { get; set; }
        public string TimeZone { get; set; }


        static public IDictionary<string, IDictionary<string, object>> ListAirports;

        static Airport()
        {
            // 04G set as default airport
            GetAirportByIata("04G");
        }
        /// <summary>
        /// Get Airport object from ICAO
        /// </summary>
        /// <param name="icao"></param>
        /// <returns></returns>
        public static Airport GetAirportByICAO(string icao)
        {


            var choosedAirport = ListAirports.Where(s => s.Value.ContainsKey("ICAO") && s.Value["ICAO"].ToString() == icao).FirstOrDefault();

            if (!String.IsNullOrEmpty(choosedAirport.Key))
            {
                return GetAirportByIata(choosedAirport.Key);
            }

            return new Airport()
            {
                Name = icao + " (no found)",
                ICAO = icao,
            };

        }
        /// <summary>
        /// Get Airport object from IATA
        /// </summary>
        /// <param name="iata">IATA</param>
        /// <returns></returns>
        public static Airport GetAirportByIata(string iata)
        {
            if (ListAirports == null)
            {
                string jsonstring = ResourceHelper.LoadExternalResource(resourceFileName);

                ListAirports = JsonConvert.DeserializeObject<IDictionary<string, IDictionary<string, object>>>(jsonstring);

                LoggingHelper.LogBehavior($">>> Done converting {resourceFileName} file ''.");
            }


            IDictionary<string, object> selectedAirport = null;

            if (ListAirports.ContainsKey(iata))
            {
                selectedAirport = ListAirports[iata];
            }
            // procurar por ICAO
            else if (!String.IsNullOrEmpty(iata))
            {
                var airportByICAO = ListAirports.Where(s => s.Value.ContainsKey("ICAO") && s.Value["ICAO"].ToString() == iata);

                if (airportByICAO.Any())
                {
                    iata = airportByICAO.FirstOrDefault().Key;
                }

            }

            if (ListAirports.ContainsKey(iata))
            {
                selectedAirport = ListAirports[iata];

                var airport = new Airport()
                {
                    City = selectedAirport["City"].ToString(),
                    Country = selectedAirport["Country"].ToString(),
                    Name = selectedAirport["Name"].ToString(),
                    IATA = iata,
                    Position = new GeoPosition(selectedAirport["Lat"].ToString(), selectedAirport["Long"].ToString()),
                    IsValid = true,
                    ICAO = selectedAirport["ICAO"].ToString(),
                    Altitude = AltitudeMetric.FromFoot(double.Parse(selectedAirport["Alt"].ToString())),
                    ListRunways = new List<Runway>()
                };



                return airport;
            }
            else
            {
                return new Airport()
                {
                    Name = iata + " (no found)",
                    IATA = iata,
                    City = String.Empty,
                    Country = String.Empty,
                    IsValid = false,
                };
            }
        }

        public override string ToString()
        {
            return this.City + " - " + this.ICAO;
        }

        public override bool Equals(object obj)
        {
            Airport compare = (Airport)obj;

            if (!String.IsNullOrEmpty(this.ICAO))
                return compare.ICAO == this.ICAO;
            else
                return compare.IATA == this.IATA;
        }

        public override int GetHashCode()
        {
            return this.ICAO.GetHashCode();
        }

    }
}
