using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Shop.Shared.Models;
using Shop.Server.Models;
using Shop.Server.Entities;
using Shop.Server.Services;
using System.Net.NetworkInformation;

namespace Shop.Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly Helpers _helpers;
        private readonly UserManager<ShopUser> _userManager;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IMapper _mapper;

        public OrdersController(
            Helpers helpers,
            UserManager<ShopUser> userManager,
            IOrdersRepository ordersRepository, 
            IProductsRepository productsRepository,
            IMapper mapper)
        {
            _helpers = helpers ?? throw new ArgumentNullException(nameof(helpers));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Get orders
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string email)
        {
            try
            {
                var orders = await _ordersRepository.GetOrders(email);

                if (orders == null) return NotFound();

                return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
            }
            catch (Exception e)
            {
                return _helpers.ErrorResponse(e);
            }
        }

        // Get one order
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            try
            {
                var order = await _ordersRepository.GetOrder(id);

                if (order == null) return NotFound();

                return Ok(_mapper.Map<OrderDto>(order));
            }
            catch (Exception e)
            {
                return _helpers.ErrorResponse(e);
            }
        }

        // Create an order
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderChangeDto orderDto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var email = user?.Email;

                if (email != null)
                {
                    var order = _mapper.Map<Order>(orderDto);
                    order.Email = email;

                    await _ordersRepository.AddOrder(order);
                    await _ordersRepository.Save();
                                        
                    return CreatedAtAction("GetOrder",
                        new { id = order.Id },
                        _mapper.Map<OrderDto>(order));
                }

                return Unauthorized();

            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("database", e.InnerException != null
                    ? e.InnerException.Message
                    : e.Message);
                return ValidationProblem();
            }
            catch (Exception e)
            {
                return _helpers.ErrorResponse(e);
            }
        }

        // Set the defined Startup details for manual validation response
        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

    }
}
