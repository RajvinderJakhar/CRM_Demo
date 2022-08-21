using CRM_Core.Utility;
using CRM_Core.ViewModel;
using CRM_Employer.Core.RepositoryService;
using CRM_Employer.DataAccess.Model;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Employer.Core.Service
{
    public class EmployerService : IEmployerService
    {
        #region FIELDS
        private readonly IEmployersRepositoryService _employersRepositoryService;
        ILog _log = LogManager.GetLogger(typeof(EmployerService));

        #endregion
        #region CONSTRUCTORS
        public EmployerService(IEmployersRepositoryService employersRepositoryService)
        {
            _employersRepositoryService = employersRepositoryService;
        }
        #endregion
        #region METHODS
        /// <summary>
        /// Add employer record on sign up.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ApiResponse<EmployerDetailsViewModel> AddEmployer(long UserID)
        {
            var result = new ApiResponse<EmployerDetailsViewModel>();
            var _addRes = AddUpdateEmployerDetails(UserID, null);
            if (_addRes.Success)
            {
                result.Data = new EmployerDetailsViewModel()
                {
                    Employer_ID = _addRes.Data.ID,
                    Office_Email = _addRes.Data.Office_Email,
                    Office_Name = _addRes.Data.Office_Name,
                    Office_Phone = _addRes.Data.Office_Phone
                };
            }
            return result;
        }

        /// <summary>
        /// Update employer details
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResponse<Employers> UpdateEmployer(long UserID, EmployersModel model)
        {
            return AddUpdateEmployerDetails(UserID, model);
        }

        private ApiResponse<Employers> AddUpdateEmployerDetails(long UserID, EmployersModel model)
        {
            var result = new ApiResponse<Employers>();
            try
            {
                var employer = _employersRepositoryService.Get(x => x.UserID == UserID);
                if(employer == null)
                {
                    employer = new Employers();
                    employer.UserID = UserID;

                }
                if (model != null)
                {
                    employer.Office_City = model.Office_City;
                    employer.Office_Email = model.Office_Email;
                    employer.Office_Phone = model.Office_Phone;
                    employer.Office_Zip = model.Office_Zip;
                    employer.Office_State = model.Office_State;
                    employer.Office_Address = model.Office_Address;
                    employer.Office_Name = model.Office_Name;
                }

                if (employer.ID < 1)
                    employer = _employersRepositoryService.Add(employer);
                else
                    employer = _employersRepositoryService.Update(employer);

                result.Data = employer;
            }
            catch(DbUpdateException dbEx)
            {
                result.AddError(dbEx.Message);
                result.Data = null;
                _log.Error(dbEx.Message, dbEx);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Data = null;
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        public ApiResponse<EmployerDetailsViewModel> Get_EmployerDetail_For_Auth(long UserID)
        {
            var result = new ApiResponse<EmployerDetailsViewModel>();
            try
            {
                var employer = _employersRepositoryService.Get(x => x.UserID == UserID);
                if (employer != null)
                {
                    result.Data = new EmployerDetailsViewModel()
                    {
                        Employer_ID = employer.ID,
                        Office_Email = employer.Office_Email,
                        Office_Name = employer.Office_Name,
                        Office_Phone = employer.Office_Phone
                    };
                }
                else
                {
                    result.Data = null;
                    result.AddError("Bad request!");
                }
            }
            catch (DbUpdateException dbEx)
            {
                result.AddError(dbEx.Message);
                result.Data = null;
                _log.Error(dbEx.Message, dbEx);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Data = null;
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        public ApiResponse<Employers> Get_EmployerDetails(long UserID, long EmployerId)
        {
            var result = new ApiResponse<Employers>();
            try
            {
                var employer = _employersRepositoryService.Get(x => x.UserID == UserID && x.ID == EmployerId);
                if (employer != null)
                {
                    result.Data = employer;
                }
                else
                {
                    result.Data = null;
                    result.AddError("Bad request!");
                }
            }
            catch (DbUpdateException dbEx)
            {
                result.AddError(dbEx.Message);
                result.Data = null;
                _log.Error(dbEx.Message, dbEx);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Data = null;
                _log.Error(ex.Message, ex);
            }
            return result;
        }
        #endregion
    }
}
