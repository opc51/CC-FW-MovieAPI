using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MovieAPI.Interfaces;
using MovieAPI.Models.Domain;
using MovieAPI.Repository;
using MovieAPI.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace MovieAPI
{
    /// <summary>
    /// .NET Core inbuilt class to build the web host
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Public constructor of the start up class
        /// </summary>
        /// <param name="configuration">The application configuration classes</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// The configuration setting for the api
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container
        /// </summary>
        /// <param name="services">The service collection that new service will be injected into</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var DbPath = System.IO.Path.Join(path, "Movies.db");

            switch (Configuration.GetRequiredSection("DatabaseMode").Value) {
                case "InMemory" : 
                    services.AddDbContext<APIContext>(opt => opt.UseInMemoryDatabase("MovieDatabase"));
                    break;
                case "Sql":
                    services.AddDbContext<APIContext>(opt => opt.UseSqlite($"Data Source={DbPath}"));
                    break;
                default: throw new Exception();
            }

            services.AddControllers();

            services.AddMediatR(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("FreeWheelMovies", new OpenApiInfo
                {
                    Title = "MoviesAPI",
                    //Version = "v1" 
                    Description = "Proof of concept API used to demostrate a variety of API skills and techniques",
                    Contact = new OpenApiContact()
                    {
                        Email = "opc51@protonmail.com",
                        Name = "David Mackie"
                    },
                    License = new OpenApiLicense() { Name = "Free for any purpose" }
                });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(xmlCommentsFile);
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IMovieService, MovieService>();
            services.AddValidatorsFromAssemblyContaining<ReviewValidator>();
        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application request pipeline </param>
        /// <param name="env">Information about the web hosting environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/FreeWheelMovies/swagger.yaml", "Movies"));
            }
            else
            {
                app.UseExceptionHandler(app =>
               {
                   app.Run(async context =>
                   {
                       context.Response.StatusCode = 500;
                       await context.Response.WriteAsync("It's not you it's me. I'm having a bit of a meltdown, give me a moment to compose myself");
                   });
               });
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            // routing decisions made here
            app.UseRouting();

            // not need for this simple app
            //app.UseAuthorization();
            //app.UseAuthentication();

            // go to the selected endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
