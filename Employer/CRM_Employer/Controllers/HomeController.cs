using CRM_Core.Utility;
using CRM_Employer.Core.Service;
using CRM_Employer.DataAccess.Model;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Employer.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class HomeController : BaseController
    {
        #region FIELDS
        private readonly IEmployerService _employerService;

        ILog _log = LogManager.GetLogger(typeof(HomeController));
        #endregion
        #region CONSTRUCTORS
        public HomeController(IEmployerService employerService)
        {
            _employerService = employerService;
        }
        #endregion
        #region ACTIONS
        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> GetEmployer()
        {
            var _response = _employerService.Get_EmployerDetails(UserID, EmployerID);
            return Ok(_response);
        }

        [Route("update-employer")]
        [HttpPost]
        public async Task<IActionResult> UpdateEmployer(EmployersModel model)
        {
            var _response = new ApiResponse<Employers>();
            if (ModelState.IsValid)
            {
                _response = _employerService.UpdateEmployer(UserID, model);
                return Ok(_response);
            }
            _response.Data = null;
            _response.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Ok(_response);
        }
        #endregion
        #region ACTIONS
        [AllowAnonymous]
        [Route("/error-development")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return NotFound();
            }

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            //Log exception
            _log.Error(exceptionHandlerFeature.Error.Message, exceptionHandlerFeature.Error);

            //return Problem(
            //    detail: exceptionHandlerFeature.Error.StackTrace,
            //    title: exceptionHandlerFeature.Error.Message);
            var _response = new ApiResponse();
            _response.AddError(exceptionHandlerFeature.Error.Message);
            _response.AddError(exceptionHandlerFeature.Error.StackTrace ?? "");
            return Ok(_response);
        }

        [AllowAnonymous]
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError([FromServices] IHostEnvironment hostEnvironment)
        {
            //Log exception
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            _log.Error(exceptionHandlerFeature.Error.Message, exceptionHandlerFeature.Error);

            //return Problem();
            var _response = new ApiResponse();
            _response.AddError("An internal server error occurred. Please try again later.");
            return Ok(_response);
        }
        #endregion
    }
}
