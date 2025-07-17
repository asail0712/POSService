using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entity;
using Common.DTO;

namespace Common.Profiles
{
    public class ProductInfoProfile : Profile
    {
        public ProductInfoProfile()
        {
            CreateMap<ProductPackageRequest, ProductPackage>();
            CreateMap<ProductPackage, ProductPackageResponse>();
            CreateMap<ProductPackage, ProductBriefResponse>();            
        }
    }

}
