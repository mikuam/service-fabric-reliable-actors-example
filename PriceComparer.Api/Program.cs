using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;

namespace PriceComparer.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceName = "PriceComparer.Api";

            try
            {
                await ServiceRuntime.RegisterServiceAsync(serviceName, context => new ApiStatelessService(context, serviceName));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
