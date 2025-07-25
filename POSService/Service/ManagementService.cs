
using AutoMapper;
using Common.DTO.Management;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Service.Interface;

using XPlan.Service;
using XPlan.Utility;
using XPlan.Utility.JWT;

namespace Service
{
    public class ManagementService : GenericService<StaffDataEntity, StaffDataRequest, StaffDataResponse, IManagementRepository>, IManagementService
    {
        static private readonly string _salt = "saltt";
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
                throw new StaffNotFoundException(request.Account);
            }

            if (staffData.PasswordHash == Utils.ComputeSha256Hash(request.Password, _salt))
            {
                loginResponse.Success   = true;
                loginResponse.Token     = new JwtTokenGenerator(_jwtOptions.Secret, _jwtOptions.Issuer, _jwtOptions.Audience)
                                            .GenerateToken(staffData.Id, staffData.Account);
                return loginResponse;
            }
            else
            {
                throw new InvalidStaffPasswordException();
            }
        }

        public async Task<LoginResponse> ChangePassword(ChangePasswordRequest request)
        {
            // 1. 檢查輸入參數
            if (string.IsNullOrWhiteSpace(request.Account) ||
                string.IsNullOrWhiteSpace(request.OldPassword) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                throw new InvalidChangePasswordRequestException();
            }

            var staffData = await _repository.GetAsync(request.Account);

            if (staffData == null)
            {
                throw new StaffNotFoundException(request.Account);
            }

            if(staffData.PasswordHash != Utils.ComputeSha256Hash(request.OldPassword, _salt))
            {
                throw new InvalidStaffPasswordException();
            }

            staffData.PasswordHash = Utils.ComputeSha256Hash(request.NewPassword, _salt);

            await _repository.UpdateAsync(staffData.Account, staffData);

            return new LoginResponse
            {
                Success = true,
                Message = "密碼修改成功",
                Token   = new JwtTokenGenerator(_jwtOptions.Secret, _jwtOptions.Issuer, _jwtOptions.Audience)
                                            .GenerateToken(staffData.Id, staffData.Account)
            };
        }

        public override async Task<StaffDataResponse> CreateAsync(StaffDataRequest request)
        {
            var entity          = _mapper.Map<StaffDataEntity>(request);
            //entity.CreatedAt    = DateTime.UtcNow;
            //entity.UpdatedAt    = DateTime.UtcNow;
            entity.PasswordHash = Utils.ComputeSha256Hash(request.Password, _salt);
            entity              = await _repository.CreateAsync(entity);

            return _mapper.Map<StaffDataResponse>(entity);
        }
    }
}

