namespace NiceAirplanesRadar
{
    public interface IMovable
    {
        double VerticalSpeed { get; }
        SpeedMetric Speed { get; }
        double Direction { get; }
    }


}
