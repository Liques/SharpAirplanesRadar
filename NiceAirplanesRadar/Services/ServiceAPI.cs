using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NiceAirplanesRadar.Util;

namespace NiceAirplanesRadar.Services
{
    internal abstract class ServiceAPI : IServiceAPI
    {
        private IDataLoader ServiceDataLoader { get; set; }
        private string CacheFileName { get; set; }
        public DateTime LastUpdate { get; private set; }
        public TimeSpan UpdateInterval { get; set; }
        protected IEnumerable<IAircraft> LastAirplanes { get; private set; }
        private bool isUpdating = false;

        public ServiceAPI(IDataLoader serviceDataLoader, string cacheFileName, TimeSpan updateInterval)
        {
            this.ServiceDataLoader = serviceDataLoader;
            this.CacheFileName = cacheFileName;
            this.UpdateInterval = updateInterval;
            this.LastUpdate = DateTime.MinValue;
        }

        protected abstract IEnumerable<IAircraft> Conversor(string data);

        public virtual async Task<IEnumerable<IAircraft>> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "")
        {
            if (this.LastUpdate + this.UpdateInterval <= DateTime.Now)
            {
                await Update(customUrl: customUrl);

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

            if (cacheEnabled && !String.IsNullOrEmpty(CacheFileName) && System.IO.File.Exists(CacheFileName) && System.IO.File.GetLastWriteTime(CacheFileName).AddMinutes(1) < DateTime.Now)
            {
                LoggingHelper.LogBehavior("> Trying to load CACHE airplane list...");
                var file = System.IO.File.OpenText(CacheFileName);
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

                jsonData = await this.ServiceDataLoader.Load(customUrl);

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
