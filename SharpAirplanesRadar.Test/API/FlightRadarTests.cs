using Xunit;
using NSubstitute;
using SharpAirplanesRadar.APIs;
using Newtonsoft.Json;
using NSubstitute.ExceptionExtensions;

namespace SharpAirplanesRadar.Test.APIs
{
    public class FlightRadarTests
    {
        public class FlightRadar24ApiTests
        {
            private FlightRadar24Api _flightRadar24Api;
            private IRadarAPI _radarApiMock;

            public FlightRadar24ApiTests()
            {
                _radarApiMock = Substitute.For<IRadarAPI>();
                _flightRadar24Api = new FlightRadar24Api();
            }

            [Fact]
            public void Serializer_ShouldReturnEmptyList_WhenDataIsEmpty()
            {
                // Arrange
                string data = "";

                // Act
                var result = _flightRadar24Api.Serializer(data);

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }

            [Fact]
            public void Serializer_ShouldReturnEmptyList_WhenDataIsInvalid()
            {
                // Arrange
                string data = "This is not a valid json";

                // Assert
                Assert.Throws<JsonReaderException>(() => _flightRadar24Api.Serializer(data));
            }

            [Fact]
            public void Serializer_ShouldReturnEmptyList_WhenDataHasNoAircrafts()
            {
                // Arrange
                string data = "{\"full_count\": 0, \"version\": 4}";

                // Act
                var result = _flightRadar24Api.Serializer(data);

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }

            [Fact]
            public void Serializer_ShouldReturnOneAircraft_WhenDataHasOneAircraft()
            {
                // Arrange
                string data = "{\"full_count\": 1, \"version\": 4, \"2fc948ff\": [\"E48854\", -16.0789, -47.9287, 157, 10975, 333, \"\", \"F-SBCN1\", \"B738\", \"PR-GUH\", 1680727448, \"BSB\", \"CGH\", \"G31445\", 0, 2176, \"GLO1445\", 0, \"GLO\"]}";

                // Act
                var result = _flightRadar24Api.Serializer(data);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Count());
                var aircraft = (Airplane)result.First(); // cast to Airplane type
                Assert.Equal("e48854", aircraft.ID);
                Assert.Equal("GLO1445", aircraft.Name);
                Assert.Equal(10975, aircraft.Altitude.Foot);
                Assert.Equal(-16.0789, aircraft.Position.Latitude);
                Assert.Equal(-47.9287, aircraft.Position.Longitude);
                Assert.Equal(333, aircraft.Speed.Knot);
                Assert.Equal(0, aircraft.VerticalSpeed);
                Assert.Equal(157, aircraft.Direction);
                Assert.Equal("PR-GUH", aircraft.Registration.Name);
                Assert.False(aircraft.IsOnGround);
                Assert.Equal("BSB", aircraft.From.IATA);
                Assert.Equal("CGH", aircraft.To.IATA);
            }
        }
    }
}