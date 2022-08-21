using CRM_Auth.DataAccess.Model;
using CRM_Core.Service;
using CRM_Core.Utility;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Auth.Core.Service
{
    public class AuthService : IAuthService
    {
        #region FIELDS
        private readonly IRemoteRequestService _remoteRequestService;

        private readonly UserManager<Accounts> _userManager;
        private readonly RoleManager<AppRoles> _roleManager;

        private readonly AppSettings _appSettings;
        //private readonly ILogger<AuthService> _logger;
        ILog _log = LogManager.GetLogger(typeof(AuthService));
        #endregion
        #region CONSTRUCTORS
        public AuthService(IRemoteRequestService remoteRequestService,
                           UserManager<Accounts> userManager,
                           RoleManager<AppRoles> roleManager,
                           IOptions<AppSettings> appSettings)
        {
            _remoteRequestService = remoteRequestService;
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
        }
        #endregion
        #region Methods
        public async Task<ApiResponse<AuthResponse>> GetJwtToken(LoginModel model)
        {
            var result = new ApiResponse<AuthResponse>();
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var ident = new ClaimsIdentity(
                              new[] {
                             new Claim(ClaimTypes.NameIdentifier, user.UserName ?? user.Email, "string"),
                             new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                             new Claim(ClaimTypes.Name, user.Id.ToString(), "string"),
                             new Claim("Email", user.Email, "string"),
                              });

                    foreach (var role in userRoles)
                    {
                        ident.AddClaim(new Claim(ClaimTypes.Role, role, "string"));
                    }

                    var _employerDetailRes = await _remoteRequestService.GetAuthorizedEmployerDetail(user.Id);
                    if(_employerDetailRes.Success)
                    {
                        var _empObjStr = JsonConvert.SerializeObject(_employerDetailRes.Data);

                        ident.AddClaim(new Claim("EmployerId", _employerDetailRes.Data.Employer_ID.ToString()));
                        ident.AddClaim(new Claim("EmployerData", _empObjStr));
                    }

                    int _tokenExpire = (12 * 60 * 60);      //Seconds
                    var expireAuth = DateTime.UtcNow.AddSeconds(_tokenExpire);
                    // Creates the signed JWT
                    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.AuthKey));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = ident,
                        Expires = expireAuth,
                        Issuer = _appSettings.AuthAPI_BaseUrl,  // "localhost",
                        Audience = _appSettings.AuthAPI_BaseUrl,    // "localhost",
                        TokenType = JWT_TokenTypes.bearer.ToString(),
                        SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature)
                    };
                    var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                    var access_token = tokenHandler.WriteToken(token);

                    result.Data = new AuthResponse()
                    {
                        AccessToken = access_token,
                        TokenExpire = _tokenExpire,
                        TokenType = JWT_TokenTypes.bearer.ToString(),
                    };
                }
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        public async Task<ApiResponse> RegisterUser(RegisterUserModel model)
        {
            var response = new ApiResponse();
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                    response.Errors.Add("User already exists!");

                Accounts user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    response.AddError("Registration failed! Please check user details and try again.");
                    return response;
                }

                //  ----    Creating roles if does not exist on first time
                if (!await _roleManager.RoleExistsAsync(UserRoles.Administrator))
                {
                    await _roleManager.CreateAsync(new AppRoles(UserRoles.Administrator));
                    if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                        await _roleManager.CreateAsync(new AppRoles(UserRoles.User));
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Employer))
                        await _roleManager.CreateAsync(new AppRoles(UserRoles.Employer));
                }

                if (await _roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                }

                if (await _roleManager.RoleExistsAsync(UserRoles.Employer))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Employer);
                }

                //Employer will be add in background running service.
                var _addEmpRes = _remoteRequestService.AddEmployerForRegisteredUser(user.Id);


                //Code for send verification email on required
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                response.AddError("Registration failed! An internal error.");
            }
            return response;
        }
        #endregion
    }
}
