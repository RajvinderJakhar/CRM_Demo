using CRM_Candidate.DataAccess;
using CRM_Candidate.DataAccess.Model;
using CRM_Core.Utility;

namespace CRM_Candidate.Core.RepositoryService
{
    public class CandidatesRepositoryService : Repository<Candidates>, ICandidatesRepositoryService
    {
        public CandidatesRepositoryService(CandidateDBContext context)
            : base(context) { }
    }

    public interface ICandidatesRepositoryService : IRepository<Candidates>
    {
    }
}
