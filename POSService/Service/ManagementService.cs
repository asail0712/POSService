
using AutoMapper;
using Common.DTO;
using Common.Entity;
using Microsoft.AspNetCore.Identity;
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
using XPlan.Utility;
using XPlan.Utility.JWT;

namespace Service
{
    public class ManagementService : GenericService<StaffData, StaffDataRequest, StaffDataResponse, IManagementRepository>, IManagementService
    {
        private readonly JwtOptions _jwtOptions;

        public ManagementService(IManagementRepository repo, IMapper mapper, JwtOptions jwtOptions)
            : base(repo, mapper)
        {
            _jwtOptions = jwtOptions;
        }
        // 這裡可以添加特定於 MenuItem 的業務邏輯方法
        // 例如：根據類別獲取餐點、根據價格範圍獲取餐點等

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var staffData               = await _repository.GetAsync(request.Account);
            LoginResponse loginResponse = new LoginResponse();

            if (staffData == null)
            {
                loginResponse.Success = false;
                loginResponse.Message = "無此帳號";
                return loginResponse;
            }

            if (staffData.PasswordHash == Utils.ComputeSha256Hash(request.Password))
            {
                loginResponse.Success   = true;
                loginResponse.Token     = new JwtTokenGenerator(_jwtOptions.Secret, _jwtOptions.Issuer, _jwtOptions.Audience)
                                            .GenerateToken(staffData.Account, staffData.PasswordHash);
                return loginResponse;
            }
            else
            {
                loginResponse.Success   = false;
                loginResponse.Message   = "密碼錯誤";
                return loginResponse;
            }
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request)
        {
            // 1. 檢查輸入參數
            if (string.IsNullOrWhiteSpace(request.Account) ||
                string.IsNullOrWhiteSpace(request.OldPassword) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "請提供完整的資料"
                };
            }

            var staffData = await _repository.GetAsync(request.Account);

            if (staffData == null)
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "找不到使用者"
                };
            }

            if(staffData.PasswordHash != Utils.ComputeSha256Hash(request.OldPassword))
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "舊密碼不正確"
                };
            }

            staffData.PasswordHash = Utils.ComputeSha256Hash(request.NewPassword);

            await _repository.UpdateAsync(staffData.Account, staffData);

            return new ChangePasswordResponse
            {
                Success = true,
                Message = "密碼修改成功"
            };
        }

        public override async Task CreateAsync(StaffDataRequest request)
        {
            var entity          = _mapper.Map<StaffData>(request);
            entity.PasswordHash = Utils.ComputeSha256Hash(request.Password);

            await _repository.CreateAsync(entity);

            return;
        }
    }
}
