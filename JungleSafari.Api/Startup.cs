using Jungle.Entities;
using Jungle.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JungleSafari.Api
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
            services.AddControllers();
            services.AddDbContext<MydbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("SqlConnString")));
            services.AddScoped<IParkRepository, ParkRepository>();
            services.AddScoped<ISafariDetailRepos, SafariRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IGateRepository, GateRepository>();
            services.AddScoped<ITouristRepository,TouristRepository>();
            services.AddScoped<IBookingRepository,BookingRepository>();
            services.AddScoped<IRepository<Payment>,PaymentRepository>();
            services.AddScoped<IRepository<IdentityProof>, IdentityProofRepository>();

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
    }
}
