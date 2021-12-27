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
            services.AddCors(); // Make sure you call this previous to AddMvc
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc();
            services.AddDbContext<DatabaseContext>(optionBuilder =>
            {
                //optionBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=FootballManagement;Trusted_Connection=True;MultipleActiveResultSets=true");
                optionBuilder.UseSqlServer(_Configuration.GetConnectionString("DBConnection")).EnableSensitiveDataLogging();
            });
            services.AddControllersWithViews();
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddCors(options => options.AddPolicy("AllowOrigin", builder =>
            {
                builder.WithOrigins("https://localhost:3000").AllowAnyMethod().AllowAnyHeader();
               
            }));

            services.AddMvc();
            services.AddCors(options => options.AddPolicy("AllowOrigin", builder =>
            {
                builder.WithOrigins("https://localhost:5000").AllowAnyMethod().AllowAnyHeader();

            }));

            services.AddMvc();

            services.AddCors(options => options.AddPolicy("AllowOrigin", builder =>
            {
               
                builder.WithOrigins("https://api.jobexp.ir").AllowAnyMethod().AllowAnyHeader();

            }));

            services.AddMvc();
            services.AddCors(options => options.AddPolicy("AllowOrigin", builder =>
            {
               
                builder.WithOrigins("https://jobexp.ir").AllowAnyMethod().AllowAnyHeader();


            }));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /* if (env.IsDevelopment())
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

             app.UseAuthorization();

             app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
             });*/
            //2
            app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
            //app.UseMvc();
            app.UseCors("ApiCorsPolicy");
            //app.UseMvc();

            //1
            /*app.UseCors(
                options => options.WithOrigins("https://jobexp.ir").AllowAnyMethod()
             );*/

            /*app.UseMvc();*/
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
