using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProtectDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataProtectDemo
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddDataProtection();

            services.AddSingleton(sp =>
                sp.GetDataProtector("tdp").ToTimeLimitedDataProtector());
            services.AddSingleton(sp => sp.GetDataProtector("dp"));
            
            services.AddDbContext<DPContext>(option =>
            {
                option.UseMySql(Configuration.GetConnectionString("mysql"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
//            Seed(app);

//            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private async void Seed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DPContext>())
                {
                    if (context.Database.GetMigrations().Any())
                    {
                        //                    await context.Database.EnsureCreatedAsync();
                        await context.Database.MigrateAsync();
                    }


                    if (!context.Products.Any())
                    {
                        await context.Products.AddAsync(new Product {Name = "apple"});
                        await context.Products.AddAsync(new Product {Name = "orange"});
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}