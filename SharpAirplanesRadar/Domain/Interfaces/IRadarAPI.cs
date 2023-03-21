using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpAirplanesRadar
{
    public interface IRadarAPI
    {

        IEnumerable<IAircraft> Serializer(string data);

        string GetUrl(GeoPosition centerPosition = null, double radiusDistanceKilometers = 100, bool cacheEnabled = true, string customUrl = "");
    }
}
