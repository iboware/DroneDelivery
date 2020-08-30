using System;
using System.Threading.Tasks;
using DroneDelivery.Lib;
using DroneDelivery.Lib.Model;

namespace DroneDelivery.CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DistanceService dst = new DistanceService();

            // Add drone depots
            await dst.AddDepot("Depot1", "Metrostrasse 12, 40235 Düsseldorf");
            await dst.AddDepot("Depot2", "Ludenberger Str. 1, 40629 Düsseldorf");

            // Add stores
            await dst.AddStore("Store1", "Willstätterstraße 24, 40549 Düsseldorf");
            await dst.AddStore("Store2", "Bilker Allee 128, 40217 Düsseldorf");
            await dst.AddStore("Store3", "Hammer Landstraße 113, 41460 Neuss");
            await dst.AddStore("Store4", "Gladbacher Str. 471, 41460 Neuss");
            await dst.AddStore("Store5", "Lise-Meitner-Straße 1, 40878 Ratingen");

            //C1
            DeliveryPlan plan1 = await dst.Calculate("Friedrichstraße 133, 40217 Düsseldorf");
            PrintPlan(plan1);

            //C2
            DeliveryPlan plan2 = await dst.Calculate("Fischerstraße 23, 40477 Düsseldorf");
            PrintPlan(plan2);

            //C3
            DeliveryPlan plan3 = await dst.Calculate("Wildenbruchstraße 2, 40545 Düsseldorf");
            PrintPlan(plan3);

            //C4
            DeliveryPlan plan4 = await dst.Calculate("Reisholzer Str. 48, 40231 Düsseldorf");
            PrintPlan(plan4);


        }

        public static void PrintPlan(DeliveryPlan plan)
        {
            Console.WriteLine($"Shortest path for Address: {plan.Customer.Address}");
            Console.WriteLine($"---------------------------------------------------------------");
            Console.WriteLine($"Depot:{plan.Depot.Address} -> Store: {plan.Store.Address} -> Customer: {plan.Customer.Address}");
            Console.WriteLine($"Total Delivery Time: {(int)plan.TotalDeliveryTime.TotalMinutes}:{plan.TotalDeliveryTime.Seconds}");
        }
    }
}
