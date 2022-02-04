using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TakeOnThis.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
           
            Host.CreateDefaultBuilder(args)
           .ConfigureLogging(logging =>
           {
               logging.ClearProviders();
               logging.AddConsole();
           })
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.ConfigureKestrel((context, options) =>
               {
#if DEBUG
                   //IPAddress current = IPAddress.Loopback;
                   IPAddress current = IPAddress.Parse("192.168.0.100");
                   options.Listen(current, 5000);

#endif
               })
               .UseStartup<Startup>();
           });
        
    }
}
