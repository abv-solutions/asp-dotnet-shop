using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Shop.Client.Models;
using Shop.Shared.Models;

namespace Shop.Client.Services
{
    public partial class SetCart : ComponentBase
    {
        [Inject]
        private State state { get; set; }
        [Inject]
        private Helpers _helpers { get; set; }
        [Inject]
        private IOrdersDataService _ordersDataService { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (state.order == null)
                await GetOrder();
        }

        public async Task GetOrder()
        {
            try
            {
                var user = (await authState).User;
                var order = await _ordersDataService.GetOrder(user?.Identity?.Name);

                if (order == null)
                    await CreateNewOrder();
                else
                    state.order = order;
            }
            catch (AccessTokenNotAvailableException e)
            {
                // Redirect is ensured only on required pages
            }
            catch (Exception e)
            {
                state.err = new Error(e.Message, true);
            }
        }

        public async Task CreateNewOrder()
        {
            var newOrder = new OrderChangeDto()
            {
                Address = "-",
                Phone = "-",
                Status = "open"
            };

            var res = await _ordersDataService.AddOrder(newOrder);

            // Success
            if (res.StatusCode == System.Net.HttpStatusCode.Created)
                state.order = JsonSerializer.Deserialize<OrderDto>(
                    await res.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            // Error
            else
                await _helpers.ErrorResponse(res);
        }
    }
}
