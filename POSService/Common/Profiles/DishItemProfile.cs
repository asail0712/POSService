using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entities;
using Common.DTO;

namespace Common.Profiles
{
    public class DishItemProfile : Profile
    {
        public DishItemProfile()
        {
            CreateMap<DishItemRequest, DishItemEntity>();
            CreateMap<DishItemEntity, DishItemResponse>();
            CreateMap<DishItemEntity, DishBriefResponse>();
        }
    }

}
