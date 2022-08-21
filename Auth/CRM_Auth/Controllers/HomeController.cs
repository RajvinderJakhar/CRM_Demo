using log4net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Auth.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        #region FIELDS
        ILog _log = LogManager.GetLogger(typeof(HomeController));
        #endregion
        #region ACTIONS
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
