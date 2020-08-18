using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Shop.Client.Resources;

namespace Shop.Client.Components
{
    public partial class SearchComponent
    {
        [Parameter]
        public EventCallback<ProductRouteParams> SearchProductEventCallback { get; set; }

        private ProductRouteParams product { get; set; } = new ProductRouteParams();
        private bool inStock { get; set; }
        private bool favourite { get; set; }

        private async Task HandleValidSubmit()
        {
            product.InStock = inStock ? true : (bool?)null;
            product.Favourite = favourite ? true : (bool?)null;

            await SearchProductEventCallback.InvokeAsync(product);
        }
    }
}
