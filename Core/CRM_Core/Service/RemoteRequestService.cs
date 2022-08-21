using CRM_Core.Constants;
using CRM_Core.Utility;
using CRM_Core.ViewModel;
using log4net;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Service
{
    public sealed class RemoteRequestService : IRemoteRequestService
    {
        #region FIELDS
        private readonly AppSettings _appSettings;
        ILog _log = LogManager.GetLogger(typeof(RemoteRequestService));
        #endregion
        #region CONSTRUCTORS
        public RemoteRequestService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Add employer record on registration 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ApiResponse AddEmployerForRegisteredUser(long UserID)
        {
            var result = new ApiResponse();
            try
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => AddEmployer(UserID));
                #pragma warning restore CS4014
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        public async Task<ApiResponse<EmployerDetailsViewModel>> GetAuthorizedEmployerDetail(long UserId)
        {
            var result = new ApiResponse<EmployerDetailsViewModel>();
            try
            {
                var _token = GetJwtToken(UserId, RemoteConstants.EmployerAPIRole, _appSettings.AuthAPI_BaseUrl, _appSettings.AuthAPI_BaseUrl);      // "localhost", "localhost");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_appSettings.API_Scheme + "://" + _appSettings.EmployerAPI_BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(RemoteConstants.TokenType, _token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("cache-control", "no-cache");

                    var _response = await client.GetAsync(RemoteConstants.EmployerServicePath + "get-employer");
                    if (_response.IsSuccessStatusCode)
                    {
                        var resStr = await _response.Content.ReadAsStringAsync();
                        _log.Debug(resStr);
                        try
                        {
                            result = JsonConvert.DeserializeObject<ApiResponse<EmployerDetailsViewModel>>(resStr);
                        }
                        catch(Exception ex1)
                        {
                            _log.Error("Error on deserialize employer data string.");
                            result.Data = null;
                            result.AddError(ex1.Message);
                        }
                        
                    }
                    else
                    {
                        var resStr = await _response.Content.ReadAsStringAsync();
                        _log.Debug("Error on get employer details for user: " + UserId);
                        _log.Error(resStr);

                        result.Data = null;
                        result.AddError(resStr);
                    }

                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred on add employer record.");
                _log.Error(ex.Message, ex);
                result.Data = null;
                result.AddError(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Add employer record for registered user.
        /// </summary>
        /// <param name="UserId"></param>
        private async void AddEmployer(long UserId)
        {
            try
            {
                var _token = GetJwtToken(UserId, RemoteConstants.EmployerAPIRole, _appSettings.AuthAPI_BaseUrl, _appSettings.EmployerAPI_BaseUrl);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_appSettings.API_Scheme + "://" + _appSettings.EmployerAPI_BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(RemoteConstants.TokenType, _token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("cache-control", "no-cache");

                    var jObj = new JObject(new JProperty("UserID", UserId));
                    var objStr = JsonConvert.SerializeObject(jObj);
                    var content = new StringContent(objStr, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var _response = await client.PostAsync(RemoteConstants.EmployerServicePath + "add-employer", content);
                    if (_response.IsSuccessStatusCode)
                    {
                        var resStr = await _response.Content.ReadAsStringAsync();
                        _log.Debug("Employer record added for user: " + UserId);
                        _log.Debug(resStr);
                    }
                    else
                    {
                        var resStr = await _response.Content.ReadAsStringAsync();
                        _log.Debug("Error on add employer for user: " + UserId);
                        _log.Error(resStr);
                    }

                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred on add employer record.");
                _log.Error(ex.Message, ex);
            }
        }

        private string GetJwtToken(long UserId, string role, string _issuer, string _aud)
        {
            try
            {
                var ident = new ClaimsIdentity(
                          new[] {
                             new Claim(ClaimTypes.NameIdentifier, UserId.ToString(), "string"),
                             new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                             new Claim(ClaimTypes.Name, UserId.ToString(), "string"),
                          });
                ident.AddClaim(new Claim(ClaimTypes.Role, role, "string"));
                int _tokenExpire = (1 * 60 * 60);      //Seconds
                var expireAuth = DateTime.UtcNow.AddSeconds(_tokenExpire);
                // Creates the signed JWT
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.AuthKey));
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = ident,
                    Expires = expireAuth,
                    Issuer = _issuer,
                    Audience = _aud,
                    TokenType = RemoteConstants.TokenType,
                    SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                var access_token = tokenHandler.WriteToken(token);

                return access_token;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
            }
            return null;
        }

        #endregion

    }
}
