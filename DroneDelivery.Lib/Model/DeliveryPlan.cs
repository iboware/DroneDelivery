using System;

namespace DroneDelivery.Lib.Model
{
    public class DeliveryPlan
    {
        public Location Customer { get; set; }
        public Store Store { get; set; }
        public DroneDepot Depot { get; set; }
        public TimeSpan TotalDeliveryTime { get; set; }
    }
}
