
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
using XPlan.Interface;
using XPlan.Service;

namespace Service
{
    public class ManagementService : GenericService<StaffData, StaffDataRequest, StaffDataResponse, IManagementRepository>, IManagementService
    {
        public ManagementService(IManagementRepository repo, IMapper mapper)
            : base(repo, mapper)
        {
        }
        // 這裡可以添加特定於 MenuItem 的業務邏輯方法
        // 例如：根據類別獲取餐點、根據價格範圍獲取餐點等

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            IEnumerable<StaffData?>? dataList   = await _repository.GetAllAsync();
            LoginResponse loginResponse         = new LoginResponse();

            if (dataList == null)
            {
                return loginResponse;
            }

            foreach (StaffData? data in dataList)
            {
                if (data == null)
                {
                    continue;
                }

                if (data.Account == request.Account)
                {
                    if (data.PasswordHash == request.PasswordHash)
                    {
                        loginResponse.IsSuccess = true;
                        loginResponse.StaffData = _mapper.Map<StaffDataResponse>(data);
                        return loginResponse;
                    }
                    else
                    {
                        loginResponse.IsSuccess     = false;
                        loginResponse.ErrorMessage  = "密碼錯誤";
                        return loginResponse;
                    }
                }
            }

            return loginResponse;
        }

        public async Task<StaffDataResponse> QueryAccount(string accountID)
        {
            IEnumerable<StaffData?>? dataList = await _repository.GetAllAsync();

            if (dataList == null)
            {
                return new StaffDataResponse();
            }

            foreach (StaffData? data in dataList)
            {
                if (data == null)
                {
                    continue;
                }

                if (data.Account == accountID)
                {
                    return  _mapper.Map<StaffDataResponse>(data);
                }
            }

            return new StaffDataResponse();
        }
    }
}
