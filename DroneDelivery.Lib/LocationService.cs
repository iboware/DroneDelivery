using System;
using System.Threading.Tasks;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System.Linq;
using DroneDelivery.Lib.Model;
using GeoJSON.Net.Geometry;

namespace DroneDelivery.Lib
{
    public class LocationService
    {
        public async Task<Location> Find(string address)
        {
            var geocoder = new ForwardGeocoder();

            GeocodeResponse[] responses = await geocoder.Geocode(new ForwardGeocodeRequest
            {
                queryString = address,

                BreakdownAddressElements = true,
                ShowExtraTags = false,
                ShowAlternativeNames = false,
                ShowGeoJSON = true,
            });
            GeocodeResponse location = responses.FirstOrDefault();

            if (location is null) throw new Exception($"Address not found: {address}");

            return new Location()
            {
                Coordinate = new Position(location.Latitude,location.Longitude),
                Address = address
            };
        }
    }
}
