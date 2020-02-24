using System;
using System.Diagnostics.CodeAnalysis;
using NiceAirplanesRadar;

namespace NiceAirplanesRadar
{
    /// <summary>
    /// Object that refers to airports runways
    /// </summary>
    public class AltitudeMetric : IComparable
    {
        private const double FootToMeter = 0.3048;
        private const double FootToMile = 0.000189393939;

        public double Foot { get; private set; }
        public double Meter { get { return this.Foot * FootToMeter; } }
        public double Mile { get { return this.Foot * FootToMile; } }
        
        private AltitudeMetric(double foot)
        {
            this.Foot = foot;
        }

        public static AltitudeMetric FromMeter(double value){
            return new AltitudeMetric(value / FootToMeter);
        }
        
        public static AltitudeMetric FromMile(double value){
            return new AltitudeMetric(value / FootToMile);
        }
        
        public static AltitudeMetric FromFoot(double value){
            return new AltitudeMetric(value);
        }
        
        public override string ToString() {
            return $"{this.Foot.ToString("#.##")} ft(s)";
        }

        public int CompareTo(object obj)
        {
            return (int)Math.Round(this.Foot * 1000);
        }
        
        public static implicit operator Double(AltitudeMetric altitudeMetric)
        {
            return altitudeMetric.Foot;
        }

    }
}
