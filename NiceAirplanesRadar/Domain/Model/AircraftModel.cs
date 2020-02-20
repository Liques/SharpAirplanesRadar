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
        private const string resourceFileName = "aircraftmodels.json";
        public string Name { get; set; }
        public string ICAO { get; set; }
        public bool IsValid { get; set; }
        public AircraftCategory Type { get; set; }
        static IDictionary<string, IDictionary<string, string>> list;

        private AircraftModel()
        {

        }

        public static AircraftModel GetByICAO(string icao)
        {
            try
            {
                if (list == null)
                {
                    string jsonstring = ResourceHelper.LoadExternalResource(resourceFileName);
                    list = JsonConvert.DeserializeObject<IDictionary<string, IDictionary<string, string>>>(jsonstring);
                    LoggingHelper.LogBehavior($">>> Done converting {resourceFileName} file ''.");
                }

                string name = String.Empty;

                if (String.IsNullOrEmpty(icao))
                    icao = string.Empty;

                var nameReg = list.Keys.Where(s => icao.StartsWith(s)).FirstOrDefault();
                nameReg = (String.IsNullOrEmpty(nameReg)) ? "" : nameReg;

                AircraftModel model = new AircraftModel();
                model.ICAO = icao;
                model.IsValid = false;

                if (list.ContainsKey(nameReg))
                {
                    model.Name = list[nameReg]["Name"];
                    model.Type = list[nameReg].ContainsKey("Type") ? (AircraftCategory)System.Enum.Parse(typeof(AircraftCategory), list[nameReg]["Type"]) : AircraftCategory.NoModel;
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
                throw new ArgumentException(resourceFileName, e);
            }

        }

        public override string ToString()
        {
            return this.Name;
        }

    }


}
