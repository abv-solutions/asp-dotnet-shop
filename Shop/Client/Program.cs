using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Client.Services;

namespace Shop.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // HttpClient without access tokens
            builder.Services.AddHttpClient("Shop.PublicAPI", client => 
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            // Http client with access tokens
            builder.Services.AddHttpClient("Shop.ServerAPI", client => 
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient("Shop.ServerAPI"));

            builder.Services.AddApiAuthorization();

            //builder.Services.AddOidcAuthentication(options =>
            //{
            //    options.ProviderOptions.Authority = "https://demo.identityserver.io/";
            //    options.ProviderOptions.ClientId = "interactive.public";
            //    options.ProviderOptions.ResponseType = "code";
            //    options.ProviderOptions.DefaultScopes.Add("api");
            //});

            builder.Services.AddSingleton<State>();
            builder.Services.AddSingleton<Helpers>();

            builder.Services.AddScoped<IProductsDataService, ProductsDataService>();
            builder.Services.AddScoped<IOrdersDataService, OrdersDataService>();

            //builder.Services.AddScoped<IProductsDataService, MockProductsDataService>();
            //builder.Services.AddScoped<IOrdersDataService, MockOrdersDataService>();
            //builder.Services.AddScoped<MockShopDbContext>();
            //builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }
    }
}
