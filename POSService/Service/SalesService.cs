using AutoMapper;
using Common.DTO;
using Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            IEnumerable<SoldItem?>? allSold = await _repository.GetByTimeAsync(request.StartTimeAt, request.EndTimeAt);
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

        public async Task<int> GetConsumptionCount(string id)
        {
            IEnumerable<SoldItem?>? allSold = await _repository.GetAllAsync();

            if (allSold == null || allSold.Count() == 0)
            {
                return 0;
            }

            int count = allSold
                .Where(x => x != null && x.ProductItemList != null)
                .SelectMany(x => x!.ProductItemList!)               // 把所有 ProductItemList 合併成一個 IEnumerable<string>
                .Count(productId => productId == id);               // 統計目標 ID 出現次數

            return count;
        }
    }
}