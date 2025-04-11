using AutoMapper;
using StoreVisitTracking.Application.DTOs.Product;
using StoreVisitTracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetAllProductDto>().ReverseMap();
            CreateMap<CreateProductDto, Product>();
        }
    }
}
