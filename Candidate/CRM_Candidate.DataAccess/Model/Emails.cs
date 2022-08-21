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
    public class Emails : EmailsModel
    {
        [Key]
        public long ID { get; set; }
        [JsonIgnore]
        public long Employer_ID { get; set; }
        [JsonIgnore]
        public long From_User_ID { get; set; }
        public DateTime Created_Date { get; set; }
    }

    public class EmailsModel
    {
        
        public string TO_Email { get; set; }
        public string? From_Email { get; set; }
        [StringLength(500)]
        public string Subject { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Body { get; set; }
        public bool IsDraft { get; set; }
    }

    public class Multiple_EmailsModel
    {
        public string TO_Emails { get; set; }
        public string? From_Email { get; set; }
        [StringLength(500)]
        public string Subject { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Body { get; set; }
        public bool IsDraft { get; set; }
    }
}
