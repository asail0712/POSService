using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.OrderRecall
{
    public class OrderRecallProfile : Profile
    {
        public OrderRecallProfile()
        {
            CreateMap<OrderRecallRequest, OrderRecallEntity>();
            CreateMap<OrderRecallEntity, OrderRecallResponse>();
        }
    }

}
