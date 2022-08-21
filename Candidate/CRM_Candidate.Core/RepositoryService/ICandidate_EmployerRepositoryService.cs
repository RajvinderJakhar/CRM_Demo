using CRM_Candidate.DataAccess;
using CRM_Candidate.DataAccess.Model;
using CRM_Core.Utility;

namespace CRM_Candidate.Core.RepositoryService
{
    public class Candidate_EmployerRepositoryService : Repository<Candidate_Employer>, ICandidate_EmployerRepositoryService
    {
        public Candidate_EmployerRepositoryService(CandidateDBContext context)
            : base(context) { }
    }

    public interface ICandidate_EmployerRepositoryService : IRepository<Candidate_Employer>
    {
    }
}
