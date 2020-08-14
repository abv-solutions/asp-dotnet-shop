﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using Shop.Client.Models;
using Shop.Shared.Models;

// Implements interface methods for model manipulation

namespace Shop.Client.Services
{
    public class OrdersDataService : IOrdersDataService
    {
        private HttpClient _secureHttp { get; set; }

        public OrdersDataService(HttpClient secureHttp)
        {
            _secureHttp = secureHttp ?? throw new ArgumentNullException(nameof(secureHttp));
        }

        public async Task<OrderDto> GetOrder(string name)
        {
            return (await _secureHttp.GetFromJsonAsync<IEnumerable<OrderDto>>($"/api/orders?email={name}"))
                .FirstOrDefault();
        }

        public async Task<HttpResponseMessage> AddOrder(OrderChangeDto order)
        {
            throw new NotImplementedException();
        }
    }
}
