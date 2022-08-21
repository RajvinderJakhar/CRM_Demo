using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM_Employer.DataAccess.Model
{
    public class Employers : EmployersModel
    {
        [Key]
        [JsonIgnore]
        public long ID { get; set; }
        [JsonIgnore]
        public long UserID { get; set; }
    }

    public class EmployersModel
    {
        public string? Office_Name { get; set; }
        [StringLength(1000)]
        public string? Office_Address { get; set; }
        public string? Office_City { get; set; }
        public string? Office_Zip { get; set; }
        public string? Office_State { get; set; }
        public string? Office_Email { get; set; }
        public string? Office_Phone { get; set; }
    }

    public class AddRegisteredEmployer
    {
        public long UserID { get; set; }
    }
}
