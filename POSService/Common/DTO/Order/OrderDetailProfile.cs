﻿using AutoMapper;
using Common.DTO.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Order
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetailRequest, OrderDetailEntity>();
            CreateMap<OrderDetailEntity, OrderDetailResponse>();
            CreateMap<OrderDetailEntity, OrderDetailDocument>();
            CreateMap<OrderDetailDocument, OrderDetailEntity>();
        }
    }

}
