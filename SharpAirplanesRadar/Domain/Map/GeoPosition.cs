using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpAirplanesRadar;
using SharpAirplanesRadar.Util;

namespace SharpAirplanesRadar
{
    public class GeoPosition
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Description { get; set; }
        public AltitudeMetric Altitude { get; set; }

        public GeoPosition(double latitude, double longitude, AltitudeMetric altitude = null)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Altitude = altitude;
        }
        
        public GeoPosition(string latitude, string longitude, AltitudeMetric altitude = null)
        {
            latitude = !String.IsNullOrEmpty(latitude) ? latitude : "0";
            longitude = !String.IsNullOrEmpty(longitude) ? longitude : "0";
            
            this.Latitude = double.Parse(latitude);
            this.Longitude = double.Parse(longitude);
            this.Altitude = altitude;
        }
        
        public double Distance(GeoPosition target) {
            var distanceKilomoters = MathHelper.GetGPSDistance(this.Latitude,target.Latitude,this.Longitude, target.Longitude);
            return distanceKilomoters < 0 ? distanceKilomoters * -1 : distanceKilomoters;
        }

        public override string ToString()
        {
            return $"Lat: {this.Latitude} Lon: {this.Longitude}";
        }

    }
}
