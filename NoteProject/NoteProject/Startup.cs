using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NoteProject.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public IConfiguration _Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.AddDbContext<DatabaseContext>(optionBuilder =>
            {
                //optionBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=FootballManagement;Trusted_Connection=True;MultipleActiveResultSets=true");
                optionBuilder.UseSqlServer(_Configuration.GetConnectionString("DBConnection")).EnableSensitiveDataLogging();
            });
            services.AddControllersWithViews();




            services.AddScoped<IDatabaseContext, DatabaseContext>();*/
           // services.AddCors(); // Make sure you call this previous to AddMvc
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc();
            services.AddDbContext<DatabaseContext>(optionBuilder =>
            {
                //optionBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=FootballManagement;Trusted_Connection=True;MultipleActiveResultSets=true");
                optionBuilder.UseSqlServer(_Configuration.GetConnectionString("DBConnection")).EnableSensitiveDataLogging();
            });
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://jobexp.ir", "https://api.jobexp.ir", "https://localhost:5000", "https://localhost:3000",
                                          "http://jobexp.ir", "http://api.jobexp.ir", "http://localhost:5000", "http://localhost:3000")
                                                          .AllowAnyHeader()
                                                          .AllowAnyMethod();
                                  });
            });
            services.AddMvc();

            services.AddControllersWithViews();
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            
            /*services.AddCors(options => options.AddPolicy(MyAllowSpecificOrigins, builder =>
            {

                builder.WithOrigins("https://jobexp.ir", "https://api.jobexp.ir", "https://localhost:5000", "https://localhost:3000").AllowAnyMethod().AllowAnyHeader();
               
            }));*/

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            
           
           

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
