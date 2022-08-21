using CRM_Core.Utility;
using CRM_Employer.Core.Service;
using CRM_Employer.DataAccess.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Employer.Controllers
{
    [Route("remote/emp-commu")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ServiceCommunicationController : BaseController
    {
        #region FIELDS
        private readonly IEmployerService _employerService;

        #endregion
        #region CONSTRUCTORS
        public ServiceCommunicationController(IEmployerService employerService)
        {
            _employerService = employerService;
        }
        #endregion
        #region Actions for backend services communication
        //[AllowAnonymous]
        [Route("add-employer")]
        [HttpPost]
        public async Task<IActionResult> AddEmployer(AddRegisteredEmployer model)
        {
            if (UserID == model.UserID)
            {
                var _response = _employerService.AddEmployer(model.UserID);
                return Ok(_response);
            }
            else
                return BadRequest();
        }

        //[AllowAnonymous]
        [Route("get-employer")]
        [HttpGet]
        public async Task<IActionResult> GetEmployerForAuth()
        {
            var _response = _employerService.Get_EmployerDetail_For_Auth(UserID);
            return Ok(_response);
        }
        #endregion

    }
}
