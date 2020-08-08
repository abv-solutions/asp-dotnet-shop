using AutoMapper;
using Shop.Shared.Models;
using Shop.Server.Entities;
using Shop.Server.Models;

// Defines mappings between models and/or entities

namespace Shop.Server.Profiles
{
    public class ProductsProfile: Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductChangeDto, Product>().ReverseMap();
        }
    }
}
