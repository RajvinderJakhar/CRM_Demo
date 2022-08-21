using CRM_Candidate.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Candidate.DataAccess
{
    public class CandidateDBContext : DbContext
    {
        public CandidateDBContext(DbContextOptions<CandidateDBContext> options)
            : base(options) { }

        public DbSet<Candidates> Candidates { get; set; }
        public DbSet<Candidate_Employer> Candidate_Employer { get; set; }
        public DbSet<Emails> Emails { get; set; }
        public DbSet<Text_Logs> Text_Logs { get; set; }
    }
}
