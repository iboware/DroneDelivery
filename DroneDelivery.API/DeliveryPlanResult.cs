using System;

namespace DroneDelivery.API
{
    public struct DeliveryTime
    {
        public int Minutes;
        public int Seconds;
    }

    public class DeliveryPlanResult
    {
        public string Depot { get; set; }
        public string Store { get; set; }
        public string Customer { get; set; }
        public DeliveryTime DeliveryTime { get; set; }

    }
}
