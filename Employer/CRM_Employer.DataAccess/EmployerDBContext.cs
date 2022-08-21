using CRM_Employer.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Employer.DataAccess
{
    public class EmployerDBContext : DbContext
    {
        public EmployerDBContext(DbContextOptions<EmployerDBContext> options)
            : base(options) { }

        public DbSet<Employers> Employers { get; set; }
        public DbSet<Email_Templates> Email_Templates { get; set; }
        public DbSet<Text_Templates> Text_Templates { get; set; }
    }
}
