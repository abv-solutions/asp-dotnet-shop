using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Shop.Client.Models;
using Shop.Client.Services;
using Shop.Shared.Models;

namespace Shop.Client.Pages
{
    public partial class Cart : IDisposable
    {
        [Inject]
        private State _state { get; set; }
        [Inject]
        private Helpers _helpers { get; set; }
        [Inject]
        private IOrdersDataService _ordersDataService { get; set; }

        private SetCart setCart { get; set; }
        private ServerValidator serverValidator { get; set; }
        private OrderChangeDto order { get; set; }
        private bool loading { get; set; }
        private bool ordered { get; set; }
        private bool showOrderForm { get; set; }

        protected override void OnInitialized()
        {
            _state.OnChange += StateHasChanged;
        }

        private async void UpdateOrderItem(OrderItemDto item)
        {
            loading = true;

            try
            {
                var updateItem = JsonSerializer.Deserialize<OrderItemChangeDto>(
                    JsonSerializer.Serialize<OrderItemDto>(item));

                var res = await _ordersDataService.UpdateOrderItem(item.Id, updateItem);

                // Success
                if (res.StatusCode == System.Net.HttpStatusCode.NoContent)
                    await setCart.GetOrder();
                // Error
                else
                    await _helpers.ErrorResponse(res);

            }
            catch (AccessTokenNotAvailableException e)
            {
                e.Redirect();
            }
            catch (Exception e)
            {
                _state.err = new Error(e.Message, false);
            }

            loading = false;
            StateHasChanged();
        }

        private async Task DeleteOrderItem(int id)
        {
            loading = true;

            try
            {
                var res = await _ordersDataService.DeleteOrderItem(id);

                // Success
                if (res.StatusCode == System.Net.HttpStatusCode.NoContent)
                    await setCart.GetOrder();
                // Error
                else
                    await _helpers.ErrorResponse(res);
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
            catch (Exception e)
            {
                _state.err = new Error(e.Message, false);
            }

            loading = false;
            StateHasChanged();
        }

        private void ToggleOrderForm()
        {
            order = new OrderChangeDto()
            {
                Address = _state.order.Address,
                Phone = _state.order.Phone,
                Status = "closed",
                OrderItems = JsonSerializer.Deserialize<List<OrderItemChangeDto>>(
                    JsonSerializer.Serialize<List<OrderItemDto>>(_state.order.OrderItems))
            };

            showOrderForm = !showOrderForm;
        }

        private async void UpdateOrder(HttpResponseMessage res)
        {
            try
            {
                // Success
                if (res.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    await setCart.CreateNewOrder();
                    ordered = true;
                    StateHasChanged();
                }
                // Validation problem
                else if (res.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    serverValidator.Validate(res, order);
                // Error
                else
                    await _helpers.ErrorResponse(res);

            }
            catch (AccessTokenNotAvailableException e)
            {
                e.Redirect();
            }
            catch (Exception e)
            {
                _state.err = new Error(e.Message, false);
            }
        }

        public void Dispose()
        {
            _state.OnChange -= StateHasChanged;

            if (!_state.err.critical)
                _state.err.message = null;
        }
    }
}
