using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NiceAirplanesRadar.Util;

namespace NiceAirplanesRadar
{
    public class AircraftModel
    {
        public string Name { get; set; }
        public string ICAO { get; set; }
        public bool IsValid { get; set; }
        public AircraftCategory Type { get; set; }
        
        private AircraftModel()
        {
          
        }

        public static AircraftModel GetByICAO(string icao)
        {
            try
            {
                StreamReader file = File.OpenText(MultiOSFileSupport.ResourcesFolder + "aircraftmodels.json");

                string jsonstring = file.ReadToEnd();

                var listNames = JsonConvert.DeserializeObject<IDictionary<string, IDictionary<string, string>>>(jsonstring);

                string name = String.Empty;

                if (String.IsNullOrEmpty(icao))
                    icao = string.Empty;

                var nameReg = listNames.Keys.Where(s => icao.StartsWith(s)).FirstOrDefault();
                nameReg = (String.IsNullOrEmpty(nameReg)) ? "" : nameReg;

                AircraftModel model = new AircraftModel();
                model.ICAO = icao;
                model.IsValid = false;

                if (listNames.ContainsKey(nameReg))
                {
                    model.Name = listNames[nameReg]["Name"];
                    model.Type = (AircraftCategory)System.Enum.Parse(typeof(AircraftCategory), listNames[nameReg]["Type"]);
                    model.IsValid = true;

                    if (model.Type == AircraftCategory.NoModel)
                        model.Type = AircraftCategory.AirplaneLow;
                }
                else
                {
                    model.Name = model.ICAO;
                }

                return model;
            }
            catch (Exception e)
            {
                throw new ArgumentException(@"\Resources\aircraftmodels.json",e);
            }

        }

        public override string ToString()
        {
            return this.Name;
        }

    }

    
}
