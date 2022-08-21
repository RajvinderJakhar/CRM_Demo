using CRM_Auth.DataAccess.Model;
using CRM_Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Auth.Core.Service
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> GetJwtToken(LoginModel model);
        Task<ApiResponse> RegisterUser(RegisterUserModel model);
    }
}
