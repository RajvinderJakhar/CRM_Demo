using CRM_Candidate.DataAccess;
using CRM_Candidate.DataAccess.Model;
using CRM_Core.Utility;

namespace CRM_Candidate.Core.RepositoryService
{
    public class EmailsRepositoryService : Repository<Emails>, IEmailsRepositoryService
    {
        public EmailsRepositoryService(CandidateDBContext context)
            : base(context) { }
    }

    public interface IEmailsRepositoryService : IRepository<Emails>
    {
    }
}
