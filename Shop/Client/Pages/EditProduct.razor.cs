using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Shop.Client.Models;
using Shop.Client.Services;

namespace Shop.Client.Pages
{
    public partial class EditProduct
    {
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        public IProductsDataService _productsDataService { get; set; }
        [Parameter]
        public string id { get; set; }

        // For custom validation handling
        private ServerValidator serverValidator { get; set; }
        private ProductChangeDto product { get; set; } = new ProductChangeDto();
        private string err { get; set; }

        private int productId;

        protected override async Task OnInitializedAsync()
        {
            int.TryParse(id, out productId);

            if (productId != 0)
            {
                try
                {
                    product = await _productsDataService.GetProduct(productId);
                }
                catch (Exception e)
                {
                    err = e.Message;
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
                    Navigation.NavigateTo("products");
                // Validation problem
                else if (res.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    serverValidator.Validate(res, product);
                // Error
                else
                {
                    var body = await res.Content.ReadAsStringAsync();
                    var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(body);
                    throw new Exception($"{problemDetails.Title} {problemDetails.Detail}");
                }

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
            catch (Exception e)
            {
                err = e.Message;
                StateHasChanged();
            }
        }
    }
}
