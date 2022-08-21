using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM_Employer.DataAccess.Model
{
    public class Text_Templates : Text_TemplatesModel
    {
        [Key]
        public long ID { get; set; }
        //public long UserID { get; set; }
        [JsonIgnore]
        public long EmployerID { get; set; }
    }

    public class Text_TemplatesModel
    {
        
        public string Name { get; set; }
        [StringLength(1500)]
        public string Text_Content { get; set; }
        
    }
}
