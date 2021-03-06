using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MovieAPI
{
    /// <summary>
    /// The program begins here
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Inbuilt NET core class. Acts as the entry poin to the API
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Build the web to host the API
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
