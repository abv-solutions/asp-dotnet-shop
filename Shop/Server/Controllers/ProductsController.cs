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

namespace Shop.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly Helpers _helpers;
        private readonly IProductsRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(
            Helpers helpers,
            IProductsRepository repository, 
            IMapper mapper)
        {
            _helpers = helpers ?? throw new ArgumentNullException(nameof(helpers));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Get all products
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductRouteParams resources)
        {
            try
            {
                var products = await _repository.GetProducts(resources);
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception e)
            {
                return _helpers.ErrorResponse(e);
            }
        }

        // Get one product
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            try
            {
                var product = await _repository.GetProduct(id);

                if (product == null) return NotFound();

                return Ok(_mapper.Map<ProductDto>(product));
            }
            catch (Exception e)
            {
                return _helpers.ErrorResponse(e);
            }
        }

        // Create a product
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] ProductChangeDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);

                _repository.AddProduct(product);
                await _repository.Save();

                return CreatedAtAction("GetProduct",
                    new { id = product.Id },
                    _mapper.Map<ProductDto>(product));
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("database", e.InnerException.Message);
                return ValidationProblem();
            }
            catch (Exception e)
            {
                return _helpers.ErrorResponse(e);
            }
        }

        // Update a product
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] ProductChangeDto productDto)
        {
            try
            {
                var product = await _repository.GetProduct(id);

                if (product == null) return NotFound();

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
                return _helpers.ErrorResponse(e);
            }
        }

        // Patch a product
        [HttpPatch("{id}")]
        [Authorize]
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
                return _helpers.ErrorResponse(e);
            }
        }

        // Delete a product
        [Authorize]
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
