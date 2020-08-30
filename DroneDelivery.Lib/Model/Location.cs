using System;
using GeoJSON.Net.Geometry;

namespace DroneDelivery.Lib.Model
{
    public class Location
    {
        public IPosition Coordinate { get; set; }
        public string Address { get; set; }
    }
}
