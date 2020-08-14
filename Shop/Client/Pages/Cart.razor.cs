using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Shop.Client.Services;
using Shop.Shared.Models;

namespace Shop.Client.Pages
{
    public partial class Cart
    {
        [Inject]
        public IOrdersDataService _ordersDataService { get; set; }
        public OrderDto order { get; set; }
        private string err { get; set; }
        private bool loading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authState = await authProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                order = await _ordersDataService.GetOrder(user?.Identity?.Name);
                loading = false;
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
            catch (Exception e)
            {
                err = e.Message;
            }
        }
    }
}
