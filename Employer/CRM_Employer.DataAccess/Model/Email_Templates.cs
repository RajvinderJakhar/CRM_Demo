using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM_Employer.DataAccess.Model
{
    public class Email_Templates : Email_TemplatesModel
    {
        [Key]
        public long ID { get; set; }
        //public long UserID { get; set; }
        [JsonIgnore]
        public long EmployerID { get; set; }
    }

    public class Email_TemplatesModel
    {
        
        public string Name { get; set; }
        [StringLength(500)]
        public string Subject { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Email_Content { get; set; }
    }
}
