using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Service;

namespace Service.Interface
{
    public interface IManagementService : IService<StaffDataRequest, StaffDataResponse>
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request);        
    }
}
