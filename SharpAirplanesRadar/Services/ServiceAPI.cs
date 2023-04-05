using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpAirplanesRadar.Util;

namespace SharpAirplanesRadar.Services
{
    internal class ServiceAPI : IServiceAPI
    {
        string cacheFileName = string.Empty;
        public DateTime LastUpdate { get; private set; }
        public TimeSpan UpdateInterval { get; set; }
        protected IEnumerable<IAircraft> LastAirplanes { get; private set; }
        private bool isUpdating = false;
        private readonly IDataLoader serviceDataLoader;
        private readonly IRadarAPI radarAPI;

        public ServiceAPI(IDataLoader serviceDataLoader, IRadarAPI radarAPI)
        {
            cacheFileName = Guid.NewGuid().ToString();
            this.serviceDataLoader = serviceDataLoader;
            this.radarAPI = radarAPI;
            this.UpdateInterval = new TimeSpan(0, 0, 10);
            this.LastUpdate = DateTime.MinValue;
        }

        public virtual async Task<IEnumerable<IAircraft>> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "")
        {
            var url = radarAPI.GetUrl(centerPosition, radiusDistanceKilometers, cacheEnabled, customUrl);

            if (this.LastUpdate + this.UpdateInterval <= DateTime.Now)
            {
                await Update(customUrl: url);

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

        public async void LoadCache(bool cacheEnabled, string customUrl = null)
        {

            if (cacheEnabled && !String.IsNullOrEmpty(cacheFileName) && System.IO.File.Exists(cacheFileName) && System.IO.File.GetLastWriteTime(cacheFileName).AddMinutes(1) < DateTime.Now)
            {
                LoggingHelper.LogBehavior("> Trying to load CACHE airplane list...");
                var file = System.IO.File.OpenText(cacheFileName);
                var cacheFileString = file.ReadToEnd();
                file.Close();
                await Update(jsonData: cacheFileString, customUrl: customUrl);
                this.LastUpdate = DateTime.MinValue;
                LoggingHelper.LogBehavior("> Done to load CACHE airplane list...");
            }
            else
            {
                LoggingHelper.LogBehavior("> No CACHE file loaded.");
                await Update(customUrl: customUrl);
            }
        }

        private async Task Update(string jsonData = null, string customUrl = null)
        {
            if (String.IsNullOrEmpty(jsonData))
            {
                if (isUpdating)
                    return;

                LoggingHelper.LogBehavior("> Trying to update airplane list...");

                isUpdating = true;

                jsonData = await this.serviceDataLoader.Load(customUrl);

                try
                {
                    if (!String.IsNullOrEmpty(cacheFileName))
                    {
                        System.IO.File.WriteAllText(cacheFileName, jsonData);
                    }
                }
                catch (Exception e)
                {
                    LoggingHelper.LogBehavior($">> Error while trying to write CACHE: {e.Message}");
                }
            }
            LoggingHelper.LogBehavior(">> Converting raw data to objects...");

            this.LastAirplanes = radarAPI.Serializer(jsonData);

            LoggingHelper.LogBehavior(">> Done converting raw data to objects.");

            this.LastUpdate = DateTime.Now;

            isUpdating = false;

            LoggingHelper.LogBehavior("> Update new list done.");
        }
    }
}
