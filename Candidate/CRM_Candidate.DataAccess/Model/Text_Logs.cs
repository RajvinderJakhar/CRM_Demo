using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM_Candidate.DataAccess.Model
{
    public class Text_Logs
    {
        [Key]
        public long ID { get; set; }
        [JsonIgnore]
        public long From_User_ID { get; set; }
        [JsonIgnore]
        public long Employer_ID { get; set; }
        public DateTime Created_Date { get; set; }
    }

    public class Text_LogsModel
    {
        public long TO_CandidateID { get; set; }
        [StringLength(1500)]
        public string Text_Message { get; set; }
        public bool IsLog { get; set; }
    }

    public class MultipleText_LogsModel
    {
        [RegularExpression(@"^[0-9,]*$", ErrorMessage = "Candidate Ids are invalid.")]
        public string TO_CandidateIDs { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9.,'\(\)\s]*$", ErrorMessage = "Please don't use special characters.")]
        [StringLength(1500)]
        public string Text_Message { get; set; }
        public bool IsLog { get; set; }
    }
}
