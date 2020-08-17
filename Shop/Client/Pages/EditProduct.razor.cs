using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Shop.Client.Models;
using Shop.Client.Services;

namespace Shop.Client.Pages
{
    public partial class EditProduct : IDisposable
    {
        [Inject]
        private State state { get; set; }
        [Inject]
        private Helpers _helpers { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private IProductsDataService _productsDataService { get; set; }

        [Parameter]
        public string id { get; set; }

        private SetCart setCart { get; set; }
        private ServerValidator serverValidator { get; set; }
        private ProductChangeDto product { get; set; } = new ProductChangeDto();

        private int productId;

        protected override async Task OnInitializedAsync()
        {
            state.OnChange += StateHasChanged;

            int.TryParse(id, out productId);

            if (productId != 0)
            {
                try
                {
                    product = await _productsDataService.GetProduct(productId);
                }
                catch (Exception e)
                {
                    state.err = new Error(e.Message, false);
                }
            }
        }

        private async void HandleValidSubmit()
        {
            try
            {
                HttpResponseMessage res;

                if (productId == 0)
                    res = await _productsDataService.AddProduct(product);
                else
                    res = await _productsDataService.UpdateProduct(productId, product);

                // Success
                if (res.StatusCode == System.Net.HttpStatusCode.Created ||
                    res.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var item = state.order.OrderItems
                        .Where(o => o.ProductId == productId)
                        .FirstOrDefault();

                    if (item != null && productId != 0)
                        await setCart.GetOrder();

                    Navigation.NavigateTo("products");

                }
                // Validation problem
                else if (res.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    serverValidator.Validate(res, product);
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
        public void Dispose()
        {
            state.OnChange -= StateHasChanged;

            if (!state.err.critical)
                state.err.message = null;
        }
    }
}
