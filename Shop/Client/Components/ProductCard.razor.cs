using Microsoft.AspNetCore.Components;
using Shop.Shared.Models;

namespace Shop.Client.Components
{
    public partial class ProductCard
    {
        [Parameter]
        public ProductDto Product { get; set; }
        [Parameter]
        public EventCallback<int> DeleteEventCallback { get; set; }
        [Parameter]
        public EventCallback<ProductDto> AddOrderItemEventCallback { get; set; }
        [Parameter]
        public bool loading {get; set; }

        private async void Delete(int id)
        {
            await DeleteEventCallback.InvokeAsync(id);
        }
        private async void AddOrderItem(ProductDto product)
        {
            await AddOrderItemEventCallback.InvokeAsync(product);
        }
    }
}
