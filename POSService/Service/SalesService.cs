using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.DTO;
using Common.Entity;
using Repository.Interface;
using Service.Interface;

using XPlan.Service;

namespace Service
{
    public class SalesService : GenericService<SoldItem, SoldItemRequest, SoldItemResponse>, ISalesService
    {
        public SalesService(ISalesRepository repo, IMapper mapper)
            : base(repo, mapper)
        {
        }
    }
}