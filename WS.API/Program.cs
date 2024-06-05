using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WS.API;

namespace WS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "生态环保文书接口";
            CreateHostBuilder(args).Build().Run();
            System.AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().ConfigureKestrel(options => {
                        //设置文件上传大小为int的最大值
                        options.Limits.MaxRequestBodySize = int.MaxValue;
                    });
                })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
