using Xunit;
using NSubstitute;
using SharpAirplanesRadar.APIs;
using Newtonsoft.Json;
using NSubstitute.ExceptionExtensions;

namespace SharpAirplanesRadar.Test.APIs
{

    public class OpenSkyApiTests
    {
        private OpenSkyApi _openSkyApi;
        private IRadarAPI _radarApiMock;

        public OpenSkyApiTests()
        {
            _radarApiMock = Substitute.For<IRadarAPI>();
            _openSkyApi = new OpenSkyApi();
        }

        [Fact]
        public void Serializer_ShouldReturnEmptyList_WhenDataIsEmpty()
        {
            // Arrange
            string data = "";

            // Act
            var result = _openSkyApi.Serializer(data);

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
            Assert.Throws<JsonReaderException>(() => _openSkyApi.Serializer(data));
        }

        [Fact]
        public void Serializer_ShouldReturnEmptyList_WhenDataHasNoAircrafts()
        {
            // Arrange
            string data = "{\"full_count\": 0, \"version\": 4}";

            // Act
            var result = _openSkyApi.Serializer(data);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Serializer_ShouldReturnOneAircraft_WhenDataHasAircrafts()
        {
            // Arrange
            string data = "{\"time\":1680734752,\"states\":[[\"e8027c\",\"LPE2068 \",\"Chile\",1680734751,1680734751,-76.8061,-12.7539,6111.24,false,183.19,320.81,-11.7,null,null,null,false,0],[\"a5a8e2\",\"N464EG \",\"United States\",1680734750,1680734751,-122.3079,47.5364,null,true,56.92,150.19,0,null,null,null,false,0],[\"ae1fa0\",\"BELLS99 \",\"United States\",1680734558,1680734560,-104.7529,39.7032,1699.26,false,21.86,333.43,-1.3,null,1722.12,null,false,0]]}";

            // Act
            var result = _openSkyApi.Serializer(data);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            var aircraft = (Airplane)result.First(); // cast to OpenSkyApi type
            Assert.Equal("e8027c", aircraft.ID);
            Assert.Equal("LPE2068", aircraft.Name);
            Assert.Equal(20050, aircraft.Altitude.Foot);
            Assert.Equal(-76.8061, aircraft.Position.Longitude);
            Assert.Equal(-12.7539, aircraft.Position.Latitude);
            Assert.Equal(356.0928725701944, aircraft.Speed.Knot);
            Assert.Equal(-11.7, aircraft.VerticalSpeed);
            Assert.Equal(320.81, aircraft.Direction);
            Assert.False(aircraft.IsOnGround);
        }
    }
}