using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MovieAPI.Interfaces;
using MovieAPI.Repository;
using MovieAPI.Services;
using System.Reflection;

namespace MovieAPI
{
    /// <summary>
    /// .NET Core inbuilt class to build the web host
    /// </summary>
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
        /// <param name="services">The servic ecollection that new service will be injected into</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<APIContext>(opt => opt.UseInMemoryDatabase("MovieDatabase"));

            services.AddControllers();

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
                                                     License = new OpenApiLicense() {  Name = "Free for any purpose" }
                                                });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(xmlCommentsFile);
            });
            services.AddScoped<IMovieService, MovieService>();
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

            app.UseHttpsRedirection();

            app.UseRouting();

            // not need for this simple app
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
