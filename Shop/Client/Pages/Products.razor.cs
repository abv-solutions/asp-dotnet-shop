using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Shop.Client.Services;
using Shop.Shared.Models;

namespace Shop.Client.Pages
{
    public partial class Products
    {
        [Inject]
        public IProductsDataService _productsDataService { get; set; }

        private List<ProductDto> products { get; set; } = new List<ProductDto>();
        private string err { get; set; }
        private bool loading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                products = (await _productsDataService.GetProducts()).ToList();
                loading = false;
            }
            catch (Exception e)
            {
                err = e.Message;
            }
        }

        private async void Delete(int id)
        {
            try
            {
                var res = await _productsDataService.DeleteProduct(id);

                // Success
                if (res.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    products.RemoveAll(p => p.Id == id);
                    StateHasChanged();
                }
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
