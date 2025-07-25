﻿using AutoMapper;
using Common.DTO.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Product
{
    public class ProductInfoProfile : Profile
    {
        public ProductInfoProfile()
        {
            CreateMap<ProductPackageRequest, ProductPackageEntity>();
            CreateMap<ProductPackageEntity, ProductBriefResponse>();
            CreateMap<ProductPackageEntity, ProductPackageResponse>();
            CreateMap<ProductPackageEntity, ProductPackageDocument>();
            CreateMap<ProductPackageDocument, ProductPackageEntity>();
        }
    }
}
