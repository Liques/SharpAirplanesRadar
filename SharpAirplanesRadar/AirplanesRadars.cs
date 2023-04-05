using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SharpAirplanesRadar.Domain.Enum;
using SharpAirplanesRadar.Util;

namespace SharpAirplanesRadar
{
    public static class AirplanesRadars
    {
        public static AirplanesRadar OpenSky()
        {
            return new AirplanesRadar(Apis.OpenSky);
        }
        public static AirplanesRadar FlightRadar24()
        {
            return new AirplanesRadar(Apis.FlightRadar24);
        }
    }
}
