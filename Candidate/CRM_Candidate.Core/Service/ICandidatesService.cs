using CRM_Candidate.DataAccess.Model;
using CRM_Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Candidate.Core.Service
{
    public interface ICandidatesService
    {
        ApiResponse<List<Candidates>> GetCandidates(long UserID, long EmployerId);
        ApiResponse<List<Candidates>> GetCandidates(long UserID, long EmployerId, string searchText, string city = "", string state = "", string zipCode = "");
        ApiResponse<Candidates> GetCandidate(long UserID, long EmployerId, long candidateId);
        ApiResponse<Candidates> AddCandidate(long UserID, long EmployerId, CandidatesModel model);
        ApiResponse<Candidates> UpdateCandidate(long UserID, long EmployerId, CandidatesModel model, long candidateId);
        ApiResponse RemoveCandidate(long UserID, long candidateId);
    }
}
