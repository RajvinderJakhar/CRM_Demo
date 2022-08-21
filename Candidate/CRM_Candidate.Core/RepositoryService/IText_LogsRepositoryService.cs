using CRM_Candidate.DataAccess;
using CRM_Candidate.DataAccess.Model;
using CRM_Core.Utility;

namespace CRM_Candidate.Core.RepositoryService
{
    public class Text_LogsRepositoryService : Repository<Text_Logs>, IText_LogsRepositoryService
    {
        public Text_LogsRepositoryService(CandidateDBContext context)
            : base(context) { }
    }

    public interface IText_LogsRepositoryService : IRepository<Text_Logs>
    {
    }
}
