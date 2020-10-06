using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Covid19Api.Models;
using Microsoft.EntityFrameworkCore.InMemory; //for method useInMemory
using Covid19Api.Services;

namespace Covid19Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
        //        services.AddDbContext<Covid19Context>(options =>
        //        options.UseSqlServer(
        //        Configuration.GetConnectionString("AzureConnection")));
        //        services.BuildServiceProvider().GetService<Covid19Context>().Database.Migrate();

            services.AddControllers();

            var connectionString = Configuration["connectionStrings:Covid19DbConnectionString"];
            services.AddDbContext<Covid19Context>(c => c.UseSqlServer(connectionString));
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICaseRepository, CaseRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Covid19Context context)
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
