﻿using AutoMapper;

using Common.DTO;
using Common.Entities;
using Repository.Interface;
using Service.Interface;
using XPlan.Service;

namespace Service
{
    public class SalesService : GenericService<OrderRecallEntity, OrderRecallRequest, OrderRecallResponse, ISalesRepository>, ISalesService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDishItemRepository _dishItemRepository;

        public SalesService(ISalesRepository repo, IMapper mapper, IProductRepository productRepository, IDishItemRepository dishItemRepository)
            : base(repo, mapper)
        {
            _productRepository  = productRepository;
            _dishItemRepository = dishItemRepository;
        }

        public async Task<List<OrderRecallResponse>> GetSalesByTime(TimeRangeSalesRequest request)
        {
            List<OrderRecallEntity>? allSold = await _repository.GetByTimeAsync(request.StartTime, request.EndTime);
            return _mapper.Map<List<OrderRecallResponse>>(allSold);
        }

        public async Task<decimal> GetProductSalesByTime(TimeRangeProductSalesRequest request)
        {
            List<OrderRecallEntity> allSold = await _repository.GetByTimeAsync(request.StartTime, request.EndTime)?? new List<OrderRecallEntity>();

            return allSold.SelectMany(soldItem => soldItem.ProductItemList ?? Enumerable.Empty<ProductBrief>())
                .Sum(productItem => productItem.Price);
        }

        public async Task AddOrderDetail(string orderId, List<string> idList, decimal totalPrice)
        {
            // 批次取得產品資料
            //var productList = await _productRepository.GetAsync(idList);
            //if (productList == null || !productList.Any())
            //{
            //    // ED TODO: Handle the case where productList is null or empty, maybe throw an exception or log an error
            //    throw new InvalidOperationException("找不到任何產品資料");
            //}

            //// 收集所有需要的 DishItem Id
            //var allDishItemIds = productList
            //                    .Where(p => p.Items != null)
            //                    .SelectMany(p => p.Items!)
            //                    .Distinct()
            //                    .ToList();

            //// 批次取得所有相關 DishItem
            //var allDishItems    = await _dishItemRepository.GetAsync(allDishItemIds);
            //var dishItemMap     = allDishItems?.ToDictionary(d => d.Id, d => d.Name) ?? new Dictionary<string, string>();

            //// 組合 ProductBrief 清單
            //var briefList = productList.Select(info => new ProductBrief
            //{
            //    Id              = info.Id,
            //    Name            = info.Name,
            //    Price           = info.OverridePrice ?? 0,
            //    MenuNameList    = info.Items?
            //                .Where(id => dishItemMap.ContainsKey(id))
            //                .Select(id => dishItemMap[id])
            //                .ToList() ?? new List<string>()
            //}).ToList();

            //// 建立 SoldItem 並儲存
            //var soldItem = new OrderRecallEntity
            //{
            //    OrderId         = orderId,
            //    ProductItemList = briefList,
            //    Price           = totalPrice
            //};

            //await _repository.CreateAsync(soldItem);
        }
    }
}