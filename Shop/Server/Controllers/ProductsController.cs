using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using Shop.Shared.Models;
using Shop.Server.Models;
using Shop.Server.Entities;
using Shop.Server.Resources;
using Shop.Server.Services;
using Microsoft.AspNetCore.Identity;

namespace Shop.Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly UserManager<ShopUser> _userManager;
        private readonly IProductsRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(
            UserManager<ShopUser> userManager,
            IProductsRepository repository, 
            IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Get all products
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductsRouteParams resources)
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user?.Email;

            var products = await _repository.GetProducts(resources);
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        // Get one product
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            var product = await _repository.GetProduct(id);

            if (product == null) return NotFound();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        // Create a product
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] ProductChangeDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);

                _repository.AddProduct(product);
                await _repository.Save();

                return CreatedAtAction("GetProduct", new { id = product.Id },
                        _mapper.Map<ProductDto>(product));
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("database", e.InnerException.Message);
                return ValidationProblem();
            }
            catch (Exception e)
            {
                return ErrorResponse(e);
            }
        }

        // Update a product
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] ProductChangeDto productDto)
        {
            try
            {
                var product = await _repository.GetProduct(id);

                if (product == null) return NotFound();

                _mapper.Map(productDto, product);
                _repository.UpdateProduct(product);
                await _repository.Save();

                return NoContent();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("database", e.InnerException.Message);
                return ValidationProblem();
            }
            catch (Exception e)
            {
                return ErrorResponse(e);
            }
        }

        // Patch a product
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(
            [FromRoute] int id,
            [FromBody] JsonPatchDocument<ProductChangeDto> patchDoc)
        {
            try
            {
                var product = await _repository.GetProduct(id);

                if (product == null) return NotFound();

                var productDto = _mapper.Map<ProductChangeDto>(product);
                productDto.Favourite = null;
                patchDoc.ApplyTo(productDto, ModelState);

                if (!TryValidateModel(productDto)) return ValidationProblem();
                productDto.Favourite = product.Favourite;

                _mapper.Map(productDto, product);
                _repository.UpdateProduct(product);
                await _repository.Save();

                return NoContent();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("database", e.InnerException.Message);
                return ValidationProblem();
            }
            catch (Exception e)
            {
                return ErrorResponse(e);
            }
        }

        // Delete a product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            try
            {
                var product = await _repository.GetProduct(id);

                if (product == null) return NotFound();

                _repository.DeleteProduct(product);
                await _repository.Save();

                return NoContent();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("database", e.InnerException.Message);
                return ValidationProblem();
            }
            catch (Exception e)
            {
                return ErrorResponse(e);
            }
        }

        // --- Helper functions ---

        // Set the defined Startup details for manual validation response
        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        // Sends error response
        public JsonResult ErrorResponse(Exception e)
        {
            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Type = $"Database problem",
                Title = $"A database error occurred.",
                Detail = e.Message,
                Instance = HttpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", HttpContext.TraceIdentifier);

            return new JsonResult(problemDetails)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ContentType = "application/problem+json; charset=utf-8"
            };
        }

    }
}
