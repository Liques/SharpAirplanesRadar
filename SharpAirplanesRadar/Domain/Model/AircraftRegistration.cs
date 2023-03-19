using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpAirplanesRadar.Util;

namespace SharpAirplanesRadar
{
    /// <summary>
    /// Registration of an airplane
    /// </summary>
    public class AircraftRegistration
    {
        private const string resourceFileName = "aircraftregistration.json";
        public string Name { get; set; }
        public string Country { get; set; }
        public bool IsValid { get; set; }

        private static IDictionary<string, string> ListOfCountries { get; set; }

        public AircraftRegistration(string registration)
        {
            try
            {
                this.Name = registration;
                this.Country = GetCountryRegistration(registration);
                this.IsValid = !String.IsNullOrEmpty(registration);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Aircraft Registration error", e);
            }
        }

        public static implicit operator AircraftRegistration(string registration)
        {
            return new AircraftRegistration(registration);
        }

        /// <summary>
        /// Gets the country owner of the registration (Example: An airplane registred as "PR-GGF" is from Brazil because it starts with "PR-"
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        private static string GetCountryRegistration(string registration)
        {
            if (ListOfCountries == null)
            {
                string jsonstring = ResourceHelper.LoadExternalResource(resourceFileName);
                ListOfCountries = JsonConvert.DeserializeObject<IDictionary<string, string>>(jsonstring);

                LoggingHelper.LogBehavior($">>> Done converting {resourceFileName} file ''.");

            }

            string country = String.Empty;
            var countryReg = ListOfCountries.Keys.Where(s => registration.StartsWith(s)).FirstOrDefault();
            countryReg = (String.IsNullOrEmpty(countryReg)) ? "" : countryReg;

            if (ListOfCountries.ContainsKey(countryReg))
            {
                country = ListOfCountries[countryReg];
            }

            return country;
        }

        public override string ToString()
        {
            return this.Name;
        }

    }


}
