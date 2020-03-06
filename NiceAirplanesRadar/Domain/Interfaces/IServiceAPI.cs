using System;
using System.Collections.Generic;

namespace NiceAirplanesRadar
{
    public interface IServiceAPI
    {
        DateTime LastUpdate { get; }
        TimeSpan UpdateInterval { get; set; }

        IEnumerable<IAircraft> GetAirplanes(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "");
        void LoadCache(bool cacheEnabled, string customUrl = null);
    }
}
