using System;
using System.Diagnostics.CodeAnalysis;
using SharpAirplanesRadar;

namespace SharpAirplanesRadar
{
    /// <summary>
    /// Object that refers to airports runways
    /// </summary>
    public class SpeedMetric : IComparable
    {
        private const double KnotToKilometerPerHour = 1.85200;
        private const double KnotToMilePerHour = 1.15077945;
        private const double KnotToMeterPerHour = 1852;

        public double Knot { get; private set; }
        public double KilometerPerHour { get { return this.Knot * KnotToKilometerPerHour; } }
        public double MeterPerHour { get { return this.Knot * KnotToMeterPerHour; } }
        public double MilePerHour { get { return this.Knot * KnotToMilePerHour; } }
        
        private SpeedMetric(double knot)
        {
            this.Knot = knot;
        }

        public static SpeedMetric FromKilometerPerHour(double value){
            return new SpeedMetric(value / KnotToKilometerPerHour);
        }
        
        public static SpeedMetric FromMilePerHour(double value){
            return new SpeedMetric(value / KnotToMilePerHour);
        }
        
        public static SpeedMetric FromKnot(double value){
            return new SpeedMetric(value);
        }
        
        public override string ToString() {
            return $"{this.Knot} knot(s)";
        }

        public int CompareTo(object obj)
        {
            return (int)Math.Round(this.Knot * 1000);
        }

        public static implicit operator Double(SpeedMetric speedMetric)
        {
            return speedMetric.Knot;
        }
    }
}
