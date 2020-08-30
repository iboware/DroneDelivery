using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DroneDelivery.Lib;
using DroneDelivery.Lib.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DroneDelivery.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DroneDeliveryController : ControllerBase
    {

        private readonly ILogger<DroneDeliveryController> _logger;
        private readonly DistanceService _distanceService;
        public DroneDeliveryController(ILogger<DroneDeliveryController> logger, DistanceService distanceService)
        {
            _logger = logger;
            _distanceService = distanceService;
        }

        [HttpGet]
        public async Task<DeliveryPlanResult> Get([Required]string address)
        {

            DeliveryPlan plan = await _distanceService.Calculate(address);

            return new DeliveryPlanResult()
            {
                Customer = plan.Customer.Address,
                Depot = plan.Depot.Address,
                Store = plan.Store.Address,
                DeliveryTime = new DeliveryTime()
                {
                    Minutes = (int)plan.TotalDeliveryTime.TotalMinutes,
                    Seconds = plan.TotalDeliveryTime.Seconds
                }
            };
        }
    }
}
