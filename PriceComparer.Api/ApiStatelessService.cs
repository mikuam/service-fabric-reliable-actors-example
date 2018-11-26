using System.Collections.Generic;
using System.Fabric;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace PriceComparer.Api
{
    public class ApiStatelessService : StatelessService
    {
        private readonly string _serviceName;

        public ApiStatelessService(StatelessServiceContext serviceContext, string serviceName)
            : base(serviceContext)
        {
            _serviceName = serviceName;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(
                    serviceContext =>
                        new HttpSysCommunicationListener(serviceContext, _serviceName, (url, listener) =>
                            new WebHostBuilder()
                                .UseHttpSys()
                                .ConfigureServices(
                                    services => services
                                        .AddSingleton<StatelessServiceContext>(serviceContext))
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                .UseStartup<Startup>()
                                .UseUrls(url)
                                .Build()),
                    "Api")
            };
        }
    }
}
