using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Dish
{
    public class DishItemProfile : Profile
    {
        public DishItemProfile()
        {
            CreateMap<DishItemRequest, DishItemEntity>();
            CreateMap<DishItemEntity, DishItemResponse>();
            CreateMap<DishItemEntity, DishBriefResponse>();
            CreateMap<DishItemEntity, DishItemDocument>();
            CreateMap<DishItemDocument, DishItemEntity>();
        }
    }

}
