using CRM_Candidate.Core.Service;
using CRM_Candidate.DataAccess.Model;
using CRM_Core.Utility;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Candidate.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CandidateController : BaseController
    {
        #region FIELDS
        private readonly ICandidatesService _candidatesService;

        ILog _log = LogManager.GetLogger(typeof(CandidateController));
        #endregion
        #region CONSTRUCTORS
        public CandidateController(ICandidatesService candidatesService)
        {
            _candidatesService = candidatesService;
        }
        #endregion
        #region ACTIONS
        [Route("{candidateId:long:min(1)}/get")]
        [HttpGet]
        public async Task<IActionResult> GetCandidate(long candidateId)
        {
            var _response = _candidatesService.GetCandidate(UserID, EmployerID, candidateId);
            return Ok(_response);
        }

        [Route("get-all")]
        [HttpGet]
        public async Task<IActionResult> GetCandidates()
        {
            var _response = _candidatesService.GetCandidates(UserID, EmployerID);
            return Ok(_response);
        }

        [Route("get-all/search")]
        [HttpGet]
        public async Task<IActionResult> GetCandidates(string searchText, string city = "", string state = "", string zipCode = "")
        {
            var _response = _candidatesService.GetCandidates(UserID, EmployerID, searchText, city: city, state: state, zipCode: zipCode);
            return Ok(_response);
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddCandidate(CandidatesModel model)
        {
            var _response = new ApiResponse<Candidates>();
            if (ModelState.IsValid)
            {
                _response = _candidatesService.AddCandidate(UserID, EmployerID, model);
                return Ok(_response);
            }
            _response.Data = null;
            _response.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Ok(_response);
        }

        [Route("{candidateId:long:min(1)}/update")]
        [HttpPost]
        public async Task<IActionResult> UpdateCandidate(long candidateId, CandidatesModel model)
        {
            var _response = new ApiResponse<Candidates>();
            if (ModelState.IsValid)
            {
                _response = _candidatesService.UpdateCandidate(UserID, EmployerID, model, candidateId);
                return Ok(_response);
            }
            _response.Data = null;
            _response.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Ok(_response);
        }
        #endregion
    }
}
