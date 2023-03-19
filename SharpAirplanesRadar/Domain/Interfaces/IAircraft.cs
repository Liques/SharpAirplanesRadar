using System;

namespace SharpAirplanesRadar
{
    public interface IAircraft : IMovable, IRealWorldObject
    {
        string ID { get; }
        AircraftRegistration Registration { get; }
        AircraftModel Model { get; }
        string Name { get; }
        DateTime DateCreation { get; }
        DateTime DateExpiration { get; set; }
        AirplaneWeight Weight { get; }
        bool IsOnGround { get; }
        IAircraft PreviousAirplane { get; set; }

        string ToString();
    }


}
