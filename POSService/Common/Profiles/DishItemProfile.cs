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
    public class DishItemProfile : Profile
    {
        public DishItemProfile()
        {
            CreateMap<DishItemRequest, DishItem>();
            CreateMap<DishItem, DishItemResponse>();
            CreateMap<DishItem, DishBriefResponse>();
        }
    }

}
