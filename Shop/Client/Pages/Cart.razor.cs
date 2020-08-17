using System;
using System.Collections.Generic;
using System.Text.Json;
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
        private State state { get; set; }
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
            state.OnChange += StateHasChanged;
        }

        private async void UpdateAmount(OrderItemDto item)
        {
            loading = true;

            try
            {
                var updateItem = JsonSerializer.Deserialize<OrderItemChangeDto>(
                    JsonSerializer.Serialize<OrderItemDto>(item));

                var res = await _ordersDataService.UpdateOrderItem(item.Id.ToString(), updateItem);

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
                state.err = new Error(e.Message, false);
            }

            loading = false;
            StateHasChanged();
        }

        private async void HandleValidSubmit()
        {
            try
            {
                var res = await _ordersDataService.UpdateOrder(state.order.Id.ToString(), order);

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
                state.err = new Error(e.Message, false);
            }
        }

        private void ToggleOrderForm()
        {
            order = new OrderChangeDto()
            {
                Address = state.order.Address,
                Phone = state.order.Phone,
                Status = "closed",
                OrderItems = JsonSerializer.Deserialize<List<OrderItemChangeDto>>(
                    JsonSerializer.Serialize<List<OrderItemDto>>(state.order.OrderItems))
            };

            showOrderForm = !showOrderForm;
        }

        public void Dispose()
        {
            state.OnChange -= StateHasChanged;

            if (!state.err.critical)
                state.err.message = null;
        }
    }
}
