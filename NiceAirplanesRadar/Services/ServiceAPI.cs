using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NiceAirplanesRadar.Util;

namespace NiceAirplanesRadar.Services
{

    internal abstract class ServiceAPI : IServiceAPI
    {
        private string URL { get; set; }
        private string CacheFileName { get; set; }
        public DateTime LastUpdate { get; private set; }
        public TimeSpan UpdateInterval { get; set; }
        protected IEnumerable<IAircraft> LastAirplanes { get; private set; }
        private bool isUpdating = false;

        public ServiceAPI(string url, string cacheFileName, TimeSpan updateInterval)
        {
            this.URL = url;
            this.CacheFileName = cacheFileName;
            this.UpdateInterval = updateInterval;
            this.LastUpdate = DateTime.MinValue;
        }

        protected abstract IEnumerable<IAircraft> Conversor(string data);

        public virtual IEnumerable<IAircraft> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "")
        {
            if (this.LastUpdate + this.UpdateInterval <= DateTime.Now)
            {
                new Thread(() =>
                   {
                       Update(customUrl: customUrl);
                   }).Start();

                if (LastAirplanes == null)
                {
                    LoadCache(cacheEnabled: cacheEnabled, customUrl: customUrl);
                }
            }

            if (radiusDistanceKilometers > 0)
            {
                return centerPosition == null ? this.LastAirplanes : this.LastAirplanes.Where(w => w.Position.Distance(centerPosition) <= radiusDistanceKilometers).ToList();
            }

            return this.LastAirplanes;
        }

        public void LoadCache(bool cacheEnabled, string customUrl = null)
        {

            if (cacheEnabled && !String.IsNullOrEmpty(CacheFileName) && System.IO.File.Exists(CacheFileName) && System.IO.File.GetLastWriteTime(CacheFileName).AddMinutes(1) < DateTime.Now)
            {
                LoggingHelper.LogBehavior("> Trying to load CACHE airplane list...");
                var file = System.IO.File.OpenText(CacheFileName);
                var cacheFileString = file.ReadToEnd();
                file.Close();
                Update(jsonData: cacheFileString, customUrl: customUrl);
                this.LastUpdate = DateTime.MinValue;
                LoggingHelper.LogBehavior("> Done to load CACHE airplane list...");
            }
            else
            {
                LoggingHelper.LogBehavior("> No CACHE file loaded.");
                Update(customUrl: customUrl);
            }
        }

        private void Update(string jsonData = null, string customUrl = null)
        {
            if (String.IsNullOrEmpty(jsonData))
            {
                if (isUpdating)
                    return;

                LoggingHelper.LogBehavior("> Trying to update airplane list...");

                isUpdating = true;

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = null;

                try
                {
                    LoggingHelper.LogBehavior(">> Trying to load data from server...");

                    var apiUrl = String.IsNullOrEmpty(customUrl) ? this.URL : customUrl;
                    response = httpClient.GetAsync(apiUrl).Result;
                    LoggingHelper.LogBehavior(">> Done load data from server.");
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Server is out.", e);
                }

                jsonData = response.Content.ReadAsStringAsync().Result;

                try
                {
                    if (!String.IsNullOrEmpty(CacheFileName))
                    {
                        System.IO.File.WriteAllText(CacheFileName, jsonData);
                    }
                }
                catch (Exception e)
                {
                    LoggingHelper.LogBehavior($">> Error while trying to write CACHE: {e.Message}");
                }
            }
            LoggingHelper.LogBehavior(">> Converting raw data to objects...");

            this.LastAirplanes = Conversor(jsonData);

            LoggingHelper.LogBehavior(">> Done converting raw data to objects.");

            this.LastUpdate = DateTime.Now;

            isUpdating = false;

            LoggingHelper.LogBehavior("> Update new list done.");
        }
    }
}
