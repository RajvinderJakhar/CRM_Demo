using CRM_Core.Utility;
using CRM_Employer.DataAccess;
using CRM_Employer.DataAccess.Model;

namespace CRM_Employer.Core.RepositoryService
{
    public class Text_TemplatesRepositoryService : Repository<Text_Templates>, IText_TemplatesRepositoryService
    {
        public Text_TemplatesRepositoryService(EmployerDBContext context)
            : base(context) { }
    }

    public interface IText_TemplatesRepositoryService : IRepository<Text_Templates>
    {
    }
}
