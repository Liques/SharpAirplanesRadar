using Microsoft.Extensions.DependencyInjection;
using SharpAirplanesRadar.APIs;
using SharpAirplanesRadar.Domain.Enum;
using SharpAirplanesRadar.Services;
using SharpAirplanesRadar.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpAirplanesRadar
{
    internal static class Startup
    {
        public static IServiceProvider Register(Apis api)
        {
            var services = new ServiceCollection();

            switch (api)
            {
                case Apis.OpenSky:
                    services.AddSingleton<IRadarAPI, OpenSkyApi>();
                    break;
                case Apis.FlightRadar24:
                    services.AddSingleton<IRadarAPI, FlightRadar24Api>();
                    break;
                case Apis.ModeSMixer2:
                    services.AddSingleton<IRadarAPI, FlightRadar24Api>();
                    break;
            }

            services.AddSingleton<IDataLoader, DataLoader>();
            services.AddSingleton<IServiceAPI, ServiceAPI>();
            return services.BuildServiceProvider();
        }
    }
}
