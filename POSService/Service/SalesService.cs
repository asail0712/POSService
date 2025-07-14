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
    public class SalesService : GenericService<SoldItem, SoldItemRequest, SoldItemResponse, ISalesRepository>, ISalesService
    {
        public SalesService(ISalesRepository repo, IMapper mapper)
            : base(repo, mapper)
        {
        }

        public async Task<int> GetTotalSalesAmount(SoldItemRequest request)
        {
            SoldItem mappItem               = _mapper.Map<SoldItem>(request);
            IEnumerable<SoldItem?>? allSold = await _repository.GetByTimeAsync(mappItem.CreatedAt, mappItem.EndTimeAt);
            int totalAmoiunt                = 0;

            if(allSold == null || allSold.Count() == 0)
            {
                return 0;
            }

            foreach (SoldItem? item in allSold)
            {
                if(item == null)
                {
                    continue;
                }

                totalAmoiunt += (int)item.Amount;
            }

            return totalAmoiunt;
        }
    }
}