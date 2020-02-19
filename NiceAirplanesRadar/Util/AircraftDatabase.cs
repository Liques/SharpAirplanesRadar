using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace NiceAirplanesRadar.Util
{
    internal class AircraftDatabase
    {
        public bool IsValid { get; set; }
        public string HexCode { get; set; }
        public String Description { get; set; }
        public String AircraftModelName { get; set; }

        static IEnumerable<AircraftDatabase> ListHexCodes = new List<AircraftDatabase>();
        static IDictionary<string, string> dataRawDic = null;

        static AircraftDatabase()
        {
            
        }

        public static void LoadAircraftDatabaseFromOpenSky(string fileName)
        {
            try
            {
                LoggingHelper.LogBehavior($">>> Loading '{fileName}'...");
                var raw = File.OpenText(fileName).ReadToEnd()
                    .Split(Environment.NewLine.ToCharArray())
                    .ToList();

                dataRawDic = raw
                            .Select(s => s.Split(","))
                            .Where(w => w.Length >= 6 && !String.IsNullOrEmpty(w[5]?.Trim()))
                            .GroupBy(g => g[0].ToLower().Trim('"'))
                            .ToDictionary(k => k.Key, v => v.FirstOrDefault()[5].Trim('"').Trim());
                
                LoggingHelper.LogBehavior($">>> Done loading '{fileName}'.");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Error loading file '{fileName}'.", e);
            }
        }


        private AircraftDatabase()
        {
            
        }

        public static AircraftDatabase GetByICAO(string icao)
        {
            if (dataRawDic == null || String.IsNullOrEmpty(icao) || !String.IsNullOrEmpty(icao) && !dataRawDic.ContainsKey(icao.ToLower()))
            {
                return null;
            }

            return new AircraftDatabase()
            {
                HexCode = icao,
                AircraftModelName = dataRawDic[icao.ToLower()],
            };

        }
    }
}