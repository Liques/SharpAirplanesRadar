using System;
using NiceAirplanesRadar.Util;

namespace NiceAirplanesRadar
{

    /// <summary>
    /// The main object to refers to a real airplane. It have the main properties of an airplane.
    /// </summary>
    public class Airplane : IAircraft
    {
        public string ID { get; private set; }
        public AircraftRegistration Registration { get; private set; }
        public AircraftModel Model { get; private set; }
        public AltitudeMetric Altitude { get { return this.Position.Altitude; } }
        public double VerticalSpeed { get; private set; }
        public SpeedMetric Speed { get; private set; }
        public double Direction { get; private set; }
        public GeoPosition Position { get; private set; }
        public string Name { get; private set; }
        public Airline Airline { get; private set; }
        public DateTime DateCreation { get; private set; }
        public DateTime DateExpiration { get; set; }
        public AirplaneWeight Weight { get; private set; }
        public Airport From { get; set; }
        public Airport To { get; set; }
        public bool IsOnGround { get; private set; }
        public IAircraft PreviousAirplane { get; set; }


        public Airplane(string hexCode, string flightName, AltitudeMetric altitude, double latitude, double longitude, SpeedMetric speed, double verticalSpeed, double direction, string from, string to, string model, string registration, bool isOnGround)
        {

            var airplaneDatabaseData = AircraftDatabase.GetByICAO(hexCode);

            this.ID = hexCode;
            this.Model = airplaneDatabaseData != null ? AircraftModel.GetByICAO(airplaneDatabaseData.AircraftModelName) : null;
            this.Direction = direction;
            this.From = Airport.GetAirportByIata(from);
            this.Name = flightName.Trim();
            this.Airline = Airline.GetAirlineByFlight(flightName);
            this.Position = new GeoPosition(latitude, longitude, altitude);
            this.Registration = new AircraftRegistration(registration);
            this.Speed = speed;
            this.To = Airport.GetAirportByIata(to);
            this.VerticalSpeed = verticalSpeed;
            this.DateCreation = DateTime.Now;
            this.IsOnGround = isOnGround;
            this.DateExpiration = DateTime.Now.AddHours(1);

        }

        public override string ToString()
        {
            return $"{this.Name} ({this.Model?.ICAO}) {this.Registration})";
        }
    }


}
