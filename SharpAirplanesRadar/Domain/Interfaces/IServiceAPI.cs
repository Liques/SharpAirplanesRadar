using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpAirplanesRadar
{
    public interface IServiceAPI
    {
        DateTime LastUpdate { get; }
        TimeSpan UpdateInterval { get; set; }
        Task<IEnumerable<IAircraft>> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "");
        void LoadCache(bool cacheEnabled, string customUrl = null);
    }
}
