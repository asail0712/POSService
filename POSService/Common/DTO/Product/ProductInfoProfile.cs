using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entities;

namespace Common.DTO.Product
{
    public class ProductInfoProfile : Profile
    {
        public ProductInfoProfile()
        {
            CreateMap<ProductPackageRequest, ProductPackageEntity>();
            CreateMap<ProductPackageEntity, ProductPackageResponse>();
            CreateMap<ProductPackageEntity, ProductBriefResponse>();            
        }
    }

}
