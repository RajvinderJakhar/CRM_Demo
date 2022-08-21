using CRM_Core.Utility;
using CRM_Candidate.Core.RepositoryService;
using CRM_Candidate.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CRM_Candidate.Core.Service
{
    public class CandidatesService : ICandidatesService
    {
        #region FIELDS
        private readonly ICandidatesRepositoryService _candidatesRepositoryService;
        private readonly ICandidate_EmployerRepositoryService _candidate_EmployerRepositoryService;

        //private readonly ILogger<CandidatesService> _logger;
        ILog _log = LogManager.GetLogger(typeof(CandidatesService));
        #endregion
        #region CONSTRUCTORS
        public CandidatesService(ICandidatesRepositoryService candidatesRepositoryService,
                                 ICandidate_EmployerRepositoryService candidate_EmployerRepositoryService)
        {
            _candidatesRepositoryService = candidatesRepositoryService;
            _candidate_EmployerRepositoryService = candidate_EmployerRepositoryService;
        }
        #endregion
        #region METHODS
        public ApiResponse<List<Candidates>> GetCandidates(long UserID, long EmployerId)
        {
            return GetCandidates(UserID, EmployerId);
        }

        public ApiResponse<List<Candidates>> GetCandidates(long UserID, long EmployerId, string searchText, string city = "", string state = "", string zipCode = "")
        {
            return GetCandidates(UserID, EmployerId, searchText: searchText, city: city, state: state, zipCode: zipCode);
        }

        private ApiResponse<List<Candidates>> GetCandidatesList(long UserID, long EmployerId, string searchText = "", string city = "", string state = "", string zipCode = "")
        {
            var result = new ApiResponse<List<Candidates>>();
            try
            {
                if (UserID > 0 && EmployerId > 0)
                {
                    var candidates = _candidatesRepositoryService.GetAll(x => 
                                    x.Candidate_Employer.Where(y => y.Employer_ID == EmployerId).Count() > 0
                                && (String.IsNullOrEmpty(searchText) || (x.First_Name.Contains(searchText)
                                                                       || x.Last_Name.Contains(searchText)
                                                                       || (x.First_Name + " " + x.Last_Name).Contains(searchText)
                                                                       || x.Email.Contains(searchText)
                                                                       || x.Company.Contains(searchText)
                                                                       ))
                                && (String.IsNullOrEmpty(city) || x.City == city)
                                && (String.IsNullOrEmpty(state) || x.State_Province == state)
                                && (String.IsNullOrEmpty(zipCode) || x.Zip == zipCode)
                            ).ToList();
                    if (candidates != null)
                        result.Data = candidates;
                }
                else
                {
                    result.Data = null;
                    result.AddError("Bad request!");
                }
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Data = null;
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        public ApiResponse<Candidates> GetCandidate(long UserID, long EmployerId, long candidateId)
        {
            return GetCandidateDetail(UserID, EmployerId, candidateId);
        }

        private ApiResponse<Candidates> GetCandidateDetail(long UserID, long EmployerId, long candidateId)
        {
            var result = new ApiResponse<Candidates>();
            try
            {
                if (UserID > 0 && EmployerId > 0)
                {
                    var candidate = _candidatesRepositoryService.Get(x => x.ID == candidateId && x.Candidate_Employer.Where(y => y.Employer_ID == EmployerId).Count() > 0);
                    if (candidate != null)
                        result.Data = candidate;
                    else
                    {
                        result.Data = null;
                        result.AddError("Bad request!");
                    }
                }
                else
                {
                    result.Data = null;
                    result.AddError("Bad request!");
                }
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Data = null;
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        public ApiResponse<Candidates> AddCandidate(long UserID, long EmployerId, CandidatesModel model)
        {
            return AddUpdateCandidate(UserID, EmployerId, model);
        }

        public ApiResponse<Candidates> UpdateCandidate(long UserID, long EmployerId, CandidatesModel model, long candidateId)
        {
            return AddUpdateCandidate(UserID, EmployerId, model, candidateId: candidateId);
        }

        private ApiResponse<Candidates> AddUpdateCandidate(long UserID, long EmployerId, CandidatesModel model, long candidateId = 0)
        {
            var result = new ApiResponse<Candidates>();
            try
            {
                if (UserID > 0 && EmployerId > 0)
                {
                    var candidate = new Candidates();
                    if (candidateId > 0)
                    {
                        candidate = _candidatesRepositoryService.Get(x => x.ID == candidateId);
                    }

                    candidate.Address = model.Address;
                    candidate.City = model.City;
                    candidate.Country = model.Country;
                    candidate.Zip = model.Zip;
                    candidate.Phone = model.Phone;
                    candidate.Company = model.Company;
                    candidate.DOB = model.DOB;
                    candidate.Email = model.Email;
                    candidate.Currently_At = model.Currently_At;
                    candidate.First_Name = model.First_Name;
                    candidate.Last_Name = model.Last_Name;
                    candidate.Notes = model.Notes;
                    candidate.Role = model.Role;
                    candidate.Source = model.Source;
                    candidate.State_Province = model.State_Province;
                    candidate.Type = model.Type;

                    if (candidate.ID < 1)
                    {
                        candidate.Created_Date = DateTime.Now;
                        candidate = _candidatesRepositoryService.Add(candidate);

                        //Update candidate and employer relationship

                        var candidate_Employer = new Candidate_Employer();
                        candidate_Employer.Candidate_ID = candidate.ID;
                        candidate_Employer.Employer_ID = EmployerId;

                        candidate_Employer = _candidate_EmployerRepositoryService.Add(candidate_Employer);
                    }
                    else
                        candidate = _candidatesRepositoryService.Update(candidate);

                    

                    result.Data = candidate;
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

        public ApiResponse RemoveCandidate(long UserID, long candidateId)
        {
            var result = new ApiResponse();
            try
            {
                //Code for check Employer

                    _candidatesRepositoryService.Delete(x => x.ID == candidateId);
            }
            catch (DbUpdateException dbEx)
            {
                result.AddError(dbEx.Message);
                _log.Error(dbEx.Message, dbEx);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        #endregion


    }
}
