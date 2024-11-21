using Microsoft.Extensions.DependencyInjection;
using Project2API.Application.Abstractions.Hubs;
using Project2API.SignalR.HubServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project2API.SignalR
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection collection)
        {
            collection.AddTransient<IProductHubService, ProductHubService>();
            collection.AddSignalR(); 
        }
    }
}
