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
        public static IServiceProvider Register(Apis api, bool containsToken = false)
        {
            var services = new ServiceCollection();

            if(containsToken)
                TokenApis(api, services);
            else
                SimpleApis(api, services);

            services.AddSingleton<IServiceAPI, ServiceAPI>();
            return services.BuildServiceProvider();
        }

        private static void SimpleApis(Apis api, ServiceCollection services)
        {
            services.AddSingleton<IDataLoader, DataLoader>();

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
        }

        private static void TokenApis(Apis api, ServiceCollection services)
        {
            switch (api)
            {
                case Apis.OpenSky:
                case Apis.FlightRadar24:
                    throw new Exception("Token access support for this API is still not available.");
                default:
                    throw new Exception("This API does not requires a token.");
            }
        }
    }
}
