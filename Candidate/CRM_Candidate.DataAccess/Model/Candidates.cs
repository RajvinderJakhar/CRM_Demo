using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM_Candidate.DataAccess.Model
{
    public class Candidates : CandidatesModel
    {
        [Key]
        public long ID { get; set; }
        public DateTime Created_Date { get; set; }

        [JsonIgnore]
        public virtual IEnumerable<Candidate_Employer> Candidate_Employer { get; set; }
    }

    public class CandidatesModel
    {
        public string First_Name { get; set; }
        public string? Last_Name { get; set; }
        public DateTime? DOB { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        [StringLength(500)]
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? State_Province { get; set; }
        public string? Country { get; set; }
        public string? Company { get; set; }
        public string? Currently_At { get; set; }
        public string? Role { get; set; }
        [StringLength(2500)]
        public string? Notes { get; set; }
        public string? Source { get; set; }
        public string? Type { get; set; }
    }

    public enum Candidate_Types
    {
        Normal,
        Backlog,
        Archived
    }
}
