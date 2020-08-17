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

namespace Shop.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly Helpers _helpers;
        private readonly IOrderItemsRepository _repository;
        private readonly IMapper _mapper;

        public OrderItemsController(
            Helpers helpers,
            IOrderItemsRepository repository, 
            IMapper mapper)
        {
            _helpers = helpers ?? throw new ArgumentNullException(nameof(helpers));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Get one order item
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItem([FromRoute] int id)
        {
            try
            {
                var item = await _repository.GetOrderItem(id);

                if (item == null) return NotFound();

                return Ok(_mapper.Map<OrderItemDto>(item));
            }
            catch (Exception e)
            {
                return _helpers.ErrorResponse(e);
            }
        }

        // Create an order item
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostOrderItem([FromBody] OrderItemChangeDto itemDto)
        {
            try
            {
                var item = _mapper.Map<OrderItem>(itemDto);

                var order = await _repository.AddOrderItem(item);
                await _repository.Save();

                return CreatedAtAction("GetOrder",
                    new { controller = "orders", id = item.OrderId },
                    _mapper.Map<OrderDto>(order));
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

        // Update an order item
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem([FromRoute] int id, [FromBody] OrderItemChangeDto itemDto)
        {
            try
            {
                var item = await _repository.GetOrderItem(id);

                if (item == null) return NotFound();

                _mapper.Map(itemDto, item);

                await _repository.UpdateOrderItem(item);
                await _repository.Save();

                return NoContent();
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
