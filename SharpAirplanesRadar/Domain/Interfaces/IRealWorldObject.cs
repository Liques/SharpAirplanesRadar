namespace SharpAirplanesRadar
{
    public interface IRealWorldObject
    {
        AltitudeMetric Altitude { get; }
        GeoPosition Position { get; }
    }


}
