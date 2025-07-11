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
    public class SoldItemProfile : Profile
    {
        public SoldItemProfile()
        {
            CreateMap<SoldItemRequest, SoldItem>();
            CreateMap<SoldItem, SoldItemResponse>();
        }
    }

}
