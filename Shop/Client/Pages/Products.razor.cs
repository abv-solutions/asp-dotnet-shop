using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Shop.Client.Models;
using Shop.Client.Resources;
using Shop.Client.Services;
using Shop.Shared.Models;

namespace Shop.Client.Pages
{
    public partial class Products: IDisposable
    {
        [Inject]
        private State state { get; set; }
        [Inject]
        private Helpers _helpers { get; set; }
        [Inject]
        private IProductsDataService _productsDataService { get; set; }
        [Inject]
        private IOrdersDataService _ordersDataService { get; set; }

        private SetCart setCart { get; set; }
        private IEnumerable<ProductDto> unfilteredProducts { get; set; } = new List<ProductDto>();
        private IEnumerable<ProductDto> products { get; set; } = new List<ProductDto>();
        private bool loading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                state.OnChange += StateHasChanged;

                unfilteredProducts = await _productsDataService.GetProducts();
                products = unfilteredProducts;
            }
            catch (Exception e)
            {
                state.err = new Error(e.Message, false);
            }

            loading = false;
        }

        private async Task SearchProduct(ProductRouteParams product)
        {
            products = await _productsDataService.GetProducts(product);
        }

        private async void AddOrderItem(ProductDto product)
        {
            loading = true;

            try
            {
                if (product.InStock)
                {
                    var orderItem = new OrderItemChangeDto()
                    {
                        Amount = 1,
                        OrderId = state.order.Id,
                        ProductId = product.Id

                    };

                    var res = await _ordersDataService.AddOrderItem(orderItem);

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

        private async void DeleteProduct(int id)
        {
            loading = true;

            try
            {
                var res = await _productsDataService.DeleteProduct(id);

                // Success
                if (res.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    products = products.Where(p => p.Id != id);
                    await setCart.GetOrder();
                }
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
                state.err = new Error(e.Message, false);
            }

            loading = false;
            StateHasChanged();
        }

        public void Dispose()
        {
            state.OnChange -= StateHasChanged;

            if (!state.err.critical)
                state.err.message = null;
        }
    }
}
