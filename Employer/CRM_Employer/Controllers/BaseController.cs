using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Employer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        #region Fields
        public long UserID
        {
            get
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    try
                    {
                        return Convert.ToInt64(User.Identity.Name);
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
                }
                else
                    return 0;

            }
        }

        public long EmployerID
        {
            get
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    try
                    {
                        return Convert.ToInt64(User.Claims.Where(x => x.Type == "EmployerId").FirstOrDefault().Value);
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
                }
                else
                    return 0;

            }
        }
        #endregion
    }
}
