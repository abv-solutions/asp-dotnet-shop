using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // HttpClient without access tokens
            builder.Services
                .AddHttpClient("Shop.PublicAPI", client => 
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            // Http client with access tokens
            builder.Services
                .AddHttpClient("Shop.ServerAPI", client => 
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient("Shop.ServerAPI"));

            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}
