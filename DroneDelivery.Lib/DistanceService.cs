using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using DroneDelivery.Lib.Model;
using DroneDelivery.Lib.Utilities;
using Dijkstra.NET.Graph;
using GeoJSON.Net.Geometry;
using Dijkstra.NET.ShortestPath;

namespace DroneDelivery.Lib
{
    public class DistanceService
    {

        private readonly IList<DroneDepot> _depots;
        private readonly IList<Store> _stores;
        private const double DRONESPEED = 16.666666666666668; // 60 km/h in m/s
        private readonly LocationService _locationService;


        public IList<DroneDepot> Depots
        {
            get { return _depots; }
        }
        public IList<Store> Stores
        {
            get { return _stores; }
        }

        public DistanceService()
        {
            _locationService = new LocationService();
            _depots = new List<DroneDepot>();
            _stores = new List<Store>();
        }

        public async Task AddDepot(string name, string address)
        {

            Location location = await _locationService.Find(address);

            _depots.Add(new DroneDepot
            {
                Name = name,
                Coordinate = location.Coordinate,
                Address = location.Address
            });
        }

        public void RemoveDepot(string name)
        {
            var depot = _depots.FirstOrDefault(x => x.Name == name);

            if (depot is null) return;

            _depots.Remove(depot);
        }

        public async Task AddStore(string name, string address)
        {

            Location location = await _locationService.Find(address);

            _stores.Add(new Store
            {
                Name = name,
                Coordinate = location.Coordinate,
                Address = location.Address
            });

        }

        public void RemoveStore(string name)
        {
            var store = _stores.FirstOrDefault(x => x.Name == name);

            if (store is null) return;

            _stores.Remove(store);
        }

        /// <summary>
        /// Calculates closest distance between customer, depot and drone.
        /// </summary>
        /// <param name="customerAddress">Address of a customer</param>
        /// <returns>DeliveryPlan, which contains routes and estimated time.</returns>
        public async Task<DeliveryPlan> Calculate(string customerAddress)
        {
            DeliveryPlan plan = new DeliveryPlan();
            Location customer = new Location();
            customer = await _locationService.Find(customerAddress);
            plan.Customer = customer;
            IList<ShortestPathResult> pathResults = new List<ShortestPathResult>();
            Dictionary<uint, DroneDepot> DepotMap = new Dictionary<uint, DroneDepot>();
            Dictionary<uint, Store> StoreMap = new Dictionary<uint, Store>();

            // Creating graph to be able to use Dijkstra algorithm to find the shortest path.
            var graph = new Graph<string, string>();
            uint customerId = graph.AddNode("Customer");

            foreach (DroneDepot depot in Depots)
            {
                uint depotId = graph.AddNode(depot.Name);
                DepotMap.Add(depotId, depot);
                foreach (Store store in _stores)
                {
                    uint storeId = graph.AddNode(store.Name);
                    StoreMap.Add(storeId, store);

                    // converting KM to M, because the cost variable is int, so the accuracy will be in meters.
                    graph.Connect(depotId, storeId, (int)(depot.Coordinate.Distance(store.Coordinate) * 1000), $"{depot.Name} -> {store.Name}");
                    graph.Connect(storeId, customerId, (int)(store.Coordinate.Distance(customer.Coordinate) * 1000), $"{store.Name} -> Customer");
                }
                pathResults.Add(graph.Dijkstra(depotId, customerId));
            }

            // Find the shortest path from path results. Depot -> Store -> Customer
            ShortestPathResult shortestPath = pathResults[0];

            for (int i = 1; i < pathResults.Count; i++)
            {
                shortestPath = pathResults[i].Distance < shortestPath.Distance ? pathResults[i] : shortestPath;
            }

            // Get chosen shortest distance units.
            IList<uint> nodeList = shortestPath.GetPath().ToList();

            plan.Depot = DepotMap.Where(x => x.Key == nodeList[0]).Select(x => x.Value).SingleOrDefault();
            plan.Store = StoreMap.Where(x => x.Key == nodeList[1]).Select(x => x.Value).SingleOrDefault();

            // Calculate delivery time.
            double hours = shortestPath.Distance / DRONESPEED;
            plan.TotalDeliveryTime = TimeSpan.FromSeconds(hours);

            return plan;
        }
    }
}
