using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Entities;
using Common.DTO.Order;

namespace Common.Profiles
{
    public class SoldItemProfile : Profile
    {
        public SoldItemProfile()
        {
            CreateMap<OrderRecallRequest, OrderRecallEntity>();
            CreateMap<OrderRecallEntity, OrderRecallResponse>();
        }
    }

}
