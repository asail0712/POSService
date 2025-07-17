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
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetailRequest, OrderDetailEntity>();
            CreateMap<OrderDetailEntity, OrderDetailResponse>();
        }
    }

}
