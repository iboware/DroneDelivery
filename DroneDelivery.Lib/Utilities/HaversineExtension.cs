using System;
using System.Threading.Tasks;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System.Linq;
using System.Collections.Generic;
using DroneDelivery.Lib.Model;
using GeoJSON.Net.Geometry;

namespace DroneDelivery.Lib.Utilities
{
    public static class Haversine

    {
        /// <summary>
        // An implementation of Haversine distance formula
        // https://en.wikipedia.org/wiki/Haversine_formula
        /// </summary>
        /// <param name="p2">Position to be mesaured</param>
        /// <returns>Distance between two positions in kilometers</returns>
        public static double Distance(this IPosition p1, IPosition p2)
        {
            double R = 6371; //In Kilometers
            var lat = (p2.Latitude - p1.Latitude).ToRadians();
            var lng = (p2.Longitude - p1.Longitude).ToRadians();
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(p1.Latitude.ToRadians()) * Math.Cos(p2.Latitude.ToRadians()) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }

    }
}