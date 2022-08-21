using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using CRM_Auth.DataAccess.Model;
using CRM_Auth.Core.Service;
using CRM_Core.Utility;
using log4net;

namespace CRM_Auth.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        #region FIELDS
        private readonly IAuthService _authService;

        ILog _log = LogManager.GetLogger(typeof(AccountController));
        #endregion
        #region CONSTRUCTORS
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion
        #region ACTIONS
        /// <summary>
        /// Registration of Employer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("sign-up")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            var _response = new ApiResponse();
            if (ModelState.IsValid)
            {
                _response = await _authService.RegisterUser(model);
                return Ok(_response);
            }
            _response.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Ok(_response);
        }


        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _authService.GetJwtToken(model);
                if (result.Success)
                    return Ok(result.Data);
                else
                    Unauthorized("Email or password is invalid!");
            }
            return Unauthorized("Email or password is invalid!");
        }


        [AllowAnonymous]
        [Route("Test")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Test()
        {
            _log.Debug("Test log...");
            //throw new Exception("Test exception...");
            return Ok("Test");
        }

        [AllowAnonymous]
        [Route("Test2")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Test2()
        {
            return Ok("Test 2");
        }


        #endregion
    }
}
