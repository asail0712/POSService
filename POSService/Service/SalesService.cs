using AutoMapper;
using Common.DTO;
using Common.Entity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Repository;
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
        private readonly IProductRepository _productRepository;
        private readonly IDishItemRepository _dishItemRepository;

        public SalesService(ISalesRepository repo, IMapper mapper, IProductRepository productRepository, IDishItemRepository dishItemRepository)
            : base(repo, mapper)
        {
            _productRepository  = productRepository;
            _dishItemRepository = dishItemRepository;
        }

        public async Task<int> GetTotalSalesAmount(SoldItemRequest request)
        {
            //IEnumerable<SoldItem?>? allSold = await _repository.GetByTimeAsync(request.StartTimeAt, request.EndTimeAt);
            int totalAmoiunt                = 0;

            //if(allSold == null || allSold.Count() == 0)
            //{
            //    return 0;
            //}

            //foreach (SoldItem? item in allSold)
            //{
            //    if(item == null)
            //    {
            //        continue;
            //    }

            //    totalAmoiunt += (int)item.Amount;
            //}

            return totalAmoiunt;
        }

        public async Task<int> GetConsumptionCount(string id)
        {
            //IEnumerable<SoldItem?>? allSold = await _repository.GetAllAsync();

            //if (allSold == null || allSold.Count() == 0)
            //{
                return 0;
            //}

            //int count = allSold
            //    .Where(x => x != null && x.ProductItemList != null)
            //    .SelectMany(x => x!.ProductItemList!)               // 把所有 ProductItemList 合併成一個 IEnumerable<string>
            //    .Count(productId => productId == id);               // 統計目標 ID 出現次數

            //return count;
        }

        public async Task AddOrderDetail(List<string> idList, decimal totalPrice)
        {
            // 批次取得產品資料
            var productList = await _productRepository.GetAsync(idList);
            if (productList == null || !productList.Any())
            {
                // ED TODO: Handle the case where productList is null or empty, maybe throw an exception or log an error
                throw new InvalidOperationException("找不到任何產品資料");
            }

            // 收集所有需要的 DishItem Id
            var allDishItemIds = productList
                                .Where(p => p.Items != null)
                                .SelectMany(p => p.Items!)
                                .Distinct()
                                .ToList();

            // 批次取得所有相關 DishItem
            var allDishItems    = await _dishItemRepository.GetAsync(allDishItemIds);
            var dishItemMap     = allDishItems?.ToDictionary(d => d.Id, d => d.Name) ?? new Dictionary<ObjectId, string>();

            // 組合 ProductBrief 清單
            var briefList = productList.Select(info => new ProductBrief
            {
                Id              = info.Id,
                Name            = info.Name,
                Price           = info.OverridePrice ?? 0,
                MenuNameList    = info.Items?
                            .Where(id => dishItemMap.ContainsKey(new ObjectId(id)))
                            .Select(id => dishItemMap[new ObjectId(id)])
                            .ToList() ?? new List<string>()
            }).ToList();

            // 建立 SoldItem 並儲存
            var soldItem = new SoldItem
            {
                ProductItemList = briefList,
                Amount          = totalPrice
            };

            await _repository.CreateAsync(soldItem);
        }
    }
}