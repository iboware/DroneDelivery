using System;
using System.Threading.Tasks;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System.Linq;
using System.Collections.Generic;
using DroneDelivery.Lib.Model;
using DroneDelivery.Lib.Utilities;

namespace DroneDelivery.Lib
{
    public class DistanceService
    {

        private IList<DroneDepot> _depots;
        private IList<Store> _stores;
        private const double DRONESPEED = 60.0;
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
            double? closest = double.MaxValue,
            current;

            Store store = new Store
            {
                Name = name,
                Coordinate = location.Coordinate,
                Address = location.Address
            };

            foreach (DroneDepot depot in _depots)
            {

                current = depot.Coordinate.Distance(location.Coordinate);
                if (current < closest)
                {
                    store.ClosestDepot = depot;
                    closest = current;
                }
            }

            _stores.Add(store);


        }

        public void RemoveStore(string name)
        {
            var store = _stores.FirstOrDefault(x => x.Name == name);

            if (store is null) return;

            _stores.Remove(store);
        }
        public async Task<DeliveryPlan> Calculate(string customerAddress)
        {
            DeliveryPlan plan = new DeliveryPlan();
            Location customer = new Location();
            customer = await _locationService.Find(customerAddress);
            plan.Customer = customer;

            double? minTotalDistance = double.MaxValue,
            currentTotalDistance;

            foreach (Store store in _stores)
            {
                currentTotalDistance = store.Coordinate.Distance(store.ClosestDepot.Coordinate) + store.Coordinate.Distance(customer.Coordinate);
                if (currentTotalDistance < minTotalDistance)
                {
                    plan.Store = store;
                    minTotalDistance = currentTotalDistance;
                }
            }

            double hours = minTotalDistance.HasValue ? minTotalDistance.Value / DRONESPEED : 0;
            plan.TotalDeliveryTime = TimeSpan.FromHours(hours);

            return plan;
        }
    }
}
