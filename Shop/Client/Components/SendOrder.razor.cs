using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Shop.Client.Models;
using Shop.Client.Services;

namespace Shop.Client.Components
{
    public partial class SendOrder
    {
        [Inject]
        private State _state { get; set; }
        [Inject]
        private IOrdersDataService _ordersDataService { get; set; }

        [Parameter]
        public OrderChangeDto Order { get; set; }
        [Parameter]
        public EventCallback<HttpResponseMessage> UpdateOrderEventCallback { get; set; }

        private ServerValidator serverValidator { get; set; }

        private async Task HandleValidSubmit()
        {
            var res = await _ordersDataService.UpdateOrder(_state.order.Id, Order);

            await UpdateOrderEventCallback.InvokeAsync(res);
        }
    }
}
