using CRM_Core.Utility;
using CRM_Employer.DataAccess;
using CRM_Employer.DataAccess.Model;

namespace CRM_Employer.Core.RepositoryService
{
    public class EmployersRepositoryService : Repository<Employers>, IEmployersRepositoryService
    {
        public EmployersRepositoryService(EmployerDBContext context)
            : base(context) { }
    }

    public interface IEmployersRepositoryService : IRepository<Employers>
    {
    }
}


