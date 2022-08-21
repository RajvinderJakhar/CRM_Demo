using CRM_Core.Utility;
using CRM_Employer.DataAccess;
using CRM_Employer.DataAccess.Model;

namespace CRM_Employer.Core.RepositoryService
{
    public class Email_TemplatesRepositoryService : Repository<Email_Templates>, IEmail_TemplatesRepositoryService
    {
        public Email_TemplatesRepositoryService(EmployerDBContext context)
            : base(context) { }
    }

    public interface IEmail_TemplatesRepositoryService : IRepository<Email_Templates>
    {
    }
}
