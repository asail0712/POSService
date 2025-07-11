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
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItemRequest, MenuItem>();
            CreateMap<MenuItem, MenuItemResponse>();
        }
    }

}
