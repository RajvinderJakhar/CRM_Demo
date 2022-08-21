using CRM_Core.Utility;
using CRM_Core.ViewModel;
using CRM_Employer.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Employer.Core.Service
{
    public interface IEmployerService
    {
        ApiResponse<EmployerDetailsViewModel> AddEmployer(long UserID);
        ApiResponse<Employers> UpdateEmployer(long UserID, EmployersModel model);
        ApiResponse<EmployerDetailsViewModel> Get_EmployerDetail_For_Auth(long UserID);
        ApiResponse<Employers> Get_EmployerDetails(long UserID, long EmployerId);
    }
}
