﻿using System;
using SharpAirplanesRadar;

namespace SharpAirplanesRadar
{
    /// <summary>
    /// Object that refers to airports runways
    /// </summary>
    public class Runway : IRealWorldObject
    {
        public GeoPosition PositionSideOne { get; private set; }
        public GeoPosition PositionSideTwo { get; private set; }
        public AltitudeMetric Altitude { get {return PositionSideOne.Altitude; }}
        public GeoPosition Position { get {return PositionSideOne; }}

        public string IsAirplaneInFinalRunway(IAircraft airplane, double direction = 0)
        {
            string name = String.Empty;
            double degreesAperture = 5;

            double finalOneDirection = MapMathHelper.GetAngle(this.PositionSideOne.Longitude, this.PositionSideTwo.Longitude, this.PositionSideOne.Latitude, this.PositionSideTwo.Latitude);
            double finalTwoDirection = MapMathHelper.GetAngle(this.PositionSideTwo.Longitude, this.PositionSideOne.Longitude, this.PositionSideTwo.Latitude, this.PositionSideOne.Latitude);
            double degreesOneFromPosition = MapMathHelper.GetAngle(this.PositionSideOne.Longitude, airplane.Position.Longitude, this.PositionSideOne.Latitude, airplane.Position.Latitude);
            double degreesTwoFromPosition = MapMathHelper.GetAngle(this.PositionSideTwo.Longitude, airplane.Position.Longitude, this.PositionSideTwo.Latitude, airplane.Position.Latitude);

            bool isTargetInAngleFromOne = finalOneDirection - degreesAperture < degreesOneFromPosition && finalOneDirection + degreesAperture > degreesOneFromPosition;
            bool isTargetInAngleFromTwo = finalTwoDirection - degreesAperture < degreesTwoFromPosition && finalTwoDirection + degreesAperture > degreesTwoFromPosition;

            if (isTargetInAngleFromOne && airplane.VerticalSpeed < 0 || isTargetInAngleFromTwo && airplane.VerticalSpeed > 0)
                name = this.PositionSideTwo.Description;
            else if (isTargetInAngleFromOne && airplane.VerticalSpeed > 0 || isTargetInAngleFromTwo && airplane.VerticalSpeed < 0)
                name = this.PositionSideOne.Description;

            return name;
        }

        public string IsAirplaneInFinalRunway(IAircraft airplane)
        {

            var direction = MapMathHelper.GetAngle(airplane.PreviousAirplane.Position.Longitude, airplane.Position.Longitude, airplane.PreviousAirplane.Position.Latitude, airplane.Position.Latitude);
            
            return this.IsAirplaneInFinalRunway(airplane,direction);
        }

    }
}
