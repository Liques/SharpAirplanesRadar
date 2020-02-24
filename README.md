# Nice Airplanes Radar
:airplane:Nice Airplanes Radar is nice and cool loader of real world air traffic. It is possible to get list of airplanes from networks like FlightRadar24, OpenSky Network and ModeSMixer2.

[![NuGet Badge](https://buildstats.info/nuget/NiceAirplanesRadar)](https://www.nuget.org/packages/NiceAirplanesRadar/)

## Sources Networks

To load real world air traffic, the Nice Airplanes Radar could load data from these networks:

| Source Networks | README |
| ------ | ------ |
| [OpenSky Network](https://opensky-network.org/) |The OpenSky Network is a community-based receiver network which has been continuously collecting air traffic surveillance data since 2013. Their API is open to the comunnity. |
| [FlightRadar24](http://www.flightradar24.com/) | The most famous receiver network. Their API is **not** open to the comunnity, they have paid plans. Their API is here for test only, do not use it in commercial/open source/personal projects. |
[ModeSMixer2](http://xdeco.org/?page_id=48) | ModeSMixer2 is console application for combining and rebroadcasting feeds with Mode-S data in a variety of formats. This software is commonly used by single receivers. |

## How to use it

It is only needed to select the network.

```csharp
var client = new AirplanesRadar(SourceAPI.OpenSky);
var listOfWorldAirplanes = client.GetAirplanes();
```

Would you like to see the airplanes around your city? It is very easy too, you just need to know what is the airport code, actually I meant, the airport [ICAO](https://en.wikipedia.org/wiki/ICAO_airport_code) code from where you wish.

```csharp
var client = new AirplanesRadar(SourceAPI.OpenSky);
// Let's see how is the air traffic in Los Angeles
var listOfWorldAirplanes = client.GetAirplanes(Airport.GetAirportByICAO("KLAX").Position);
```
