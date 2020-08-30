using System.Threading.Tasks;
using DroneDelivery.Lib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace DroneDelivery.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            });

            // initialize and configure distance service
            DistanceService distanceService = new DistanceService();
            Task.Run(async () => await ConfigureDistanceService(distanceService));

            services.AddSingleton(distanceService);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        public async Task ConfigureDistanceService(DistanceService distanceService)
        {
            // Add depots
            await distanceService.AddDepot("Depot1", "Metrostrasse 12, 40235 Düsseldorf");
            await distanceService.AddDepot("Depot2", "Ludenberger Str. 1, 40629 Düsseldorf");

            // Add stores
            await distanceService.AddStore("Store1", "Willstätterstraße 24, 40549 Düsseldorf");
            await distanceService.AddStore("Store2", "Bilker Allee 128, 40217 Düsseldorf");
            await distanceService.AddStore("Store3", "Hammer Landstraße 113, 41460 Neuss");
            await distanceService.AddStore("Store4", "Gladbacher Str. 471, 41460 Neuss");
            await distanceService.AddStore("Store5", "Lise-Meitner-Straße 1, 40878 Ratingen");
        }
    }
}
