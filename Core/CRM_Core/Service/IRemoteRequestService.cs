using CRM_Core.Utility;
using CRM_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Service
{
    public interface IRemoteRequestService
    {
        ApiResponse AddEmployerForRegisteredUser(long UserID);
        Task<ApiResponse<EmployerDetailsViewModel>> GetAuthorizedEmployerDetail(long UserId);
    }
}
