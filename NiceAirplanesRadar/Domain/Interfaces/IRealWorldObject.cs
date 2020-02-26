namespace NiceAirplanesRadar
{
    public interface IRealWorldObject
    {
        AltitudeMetric Altitude { get; }
        GeoPosition Position { get; }
    }


}
