using AutoMapper;
using Shop.API.Entities;
using Shop.API.Models;

// Defines mappings between models and/or entities

namespace Shop.API.Profiles
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
