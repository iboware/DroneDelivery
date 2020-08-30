using System;

namespace DroneDelivery.Lib.Model
{
    public class Store : Location
    {
        public string Name { get; set; }
        public DroneDepot ClosestDepot { get; set; }
    }
}
