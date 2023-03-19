using SharpAirplanesRadar.Services;

namespace SharpAirplanesRadar
{
    public static class SourceAPI
    {
        public static IServiceAPI OpenSky { get { return new OpenSkyService(); } }
        public static IServiceAPI FlightRadar24 { get { return new FlightRadar24Service(); } }
        public static IServiceAPI ModeSMixer2 { get { return new ModeSMixer2Service(); } }
    }
}
