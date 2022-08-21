using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Candidate.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class HomeController : BaseController
    {
        #region FIELDS
        ILog _log = LogManager.GetLogger(typeof(HomeController));
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

            return Problem(
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);
        }

        [AllowAnonymous]
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError([FromServices] IHostEnvironment hostEnvironment)
        {
            //Log exception
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            _log.Error(exceptionHandlerFeature.Error.Message, exceptionHandlerFeature.Error);

            return Problem();
        }
        #endregion
    }
}
