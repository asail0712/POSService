using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO.Management
{
    public class StaffDataProfile : Profile
    {
        public StaffDataProfile()
        {
            CreateMap<StaffDataRequest, StaffDataEntity>();
            CreateMap<StaffDataEntity, StaffDataResponse>();
        }
    }

}
