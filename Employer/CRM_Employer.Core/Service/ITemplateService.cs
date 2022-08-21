using CRM_Core.Utility;
using CRM_Employer.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Employer.Core.Service
{
    public interface ITemplateService
    {
        ApiResponse<Email_Templates> AddEmailTemplate(long UserID, Email_TemplatesModel model);
        ApiResponse<Email_Templates> UpdateEmailTemplate(long UserID, Email_TemplatesModel model, long emailTemplateId);
        ApiResponse RemoveEmailTemplate(long UserID, long emailTemplateId);

        ApiResponse<Text_Templates> AddTextTemplate(long UserID, Text_TemplatesModel model);
        ApiResponse<Text_Templates> UpdateTextTemplate(long UserID, Text_TemplatesModel model, long textTemplateId);
        ApiResponse RemoveTextTemplate(long UserID, long textTemplateId);
    }
}
