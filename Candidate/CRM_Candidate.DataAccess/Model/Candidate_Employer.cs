using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM_Candidate.DataAccess.Model
{
    public class Candidate_Employer : Candidate_EmployerModel
    {
        [Key]
        public long ID { get; set; }

        [JsonIgnore]
        public virtual Candidates Candidates { get; set; }
    }

    public class Candidate_EmployerModel
    {
        [ForeignKey("Candidates")]
        public long Candidate_ID { get; set; }
        public long Employer_ID { get; set; }
    }
}
