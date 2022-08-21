using CRM_Core.Utility;
using CRM_Employer.Core.RepositoryService;
using CRM_Employer.DataAccess.Model;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Employer.Core.Service
{
    public class TemplateService : ITemplateService
    {
        #region FIELDS
        private readonly IEmployersRepositoryService _employersRepositoryService;
        private readonly IEmail_TemplatesRepositoryService _email_TemplatesRepositoryService;
        private readonly IText_TemplatesRepositoryService _text_TemplatesRepositoryService;

        //private readonly ILogger<TemplateService> _logger;
        ILog _log = LogManager.GetLogger(typeof(TemplateService));
        #endregion
        #region CONSTRUCTORS
        public TemplateService(IEmail_TemplatesRepositoryService email_TemplatesRepositoryService,
                               IText_TemplatesRepositoryService text_TemplatesRepositoryService,
                               IEmployersRepositoryService employersRepositoryService)
        {
            _email_TemplatesRepositoryService = email_TemplatesRepositoryService;
            _text_TemplatesRepositoryService = text_TemplatesRepositoryService;
            _employersRepositoryService = employersRepositoryService;
        }
        #endregion
        #region EMAIL TEMPLATE METHODS
        public ApiResponse<Email_Templates> AddEmailTemplate(long UserID, Email_TemplatesModel model)
        {
            return AddUpdateEmailTemplate(UserID, model);
        }

        public ApiResponse<Email_Templates> UpdateEmailTemplate(long UserID, Email_TemplatesModel model, long emailTemplateId)
        {
            return AddUpdateEmailTemplate(UserID, model, emailTemplateId: emailTemplateId);
        }

        private ApiResponse<Email_Templates> AddUpdateEmailTemplate(long UserID, Email_TemplatesModel model, long emailTemplateId = 0)
        {
            var result = new ApiResponse<Email_Templates>();
            try
            {
                var employer = _employersRepositoryService.Get(x => x.UserID == UserID);
                if (employer != null)
                {
                    var email_Template = new Email_Templates();
                    if(emailTemplateId > 0)
                    {
                        email_Template = _email_TemplatesRepositoryService.Get(x => x.ID == emailTemplateId && x.EmployerID == employer.ID);
                    }

                    email_Template.Email_Content = model.Email_Content;
                    email_Template.Subject = model.Subject;
                    email_Template.Name = model.Name;
                    email_Template.EmployerID = employer.ID;

                    if (email_Template.ID < 1)
                        email_Template = _email_TemplatesRepositoryService.Add(email_Template);
                    else
                        email_Template = _email_TemplatesRepositoryService.Update(email_Template);

                    result.Data = email_Template;
                }
            }
            catch (DbUpdateException dbEx)
            {
                result.AddError(dbEx.Message);
                result.Data = null;
                _log.Error(dbEx.Message, dbEx);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Data = null;
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        public ApiResponse RemoveEmailTemplate(long UserID, long emailTemplateId)
        {
            var result = new ApiResponse();
            try
            {
                var employer = _employersRepositoryService.Get(x => x.UserID == UserID);
                if (employer != null)
                {
                    _email_TemplatesRepositoryService.Delete(x => x.ID == emailTemplateId && x.EmployerID == employer.ID);
                }
                else
                    result.AddError("Bad request!");
            }
            catch (DbUpdateException dbEx)
            {
                result.AddError(dbEx.Message);
                _log.Error(dbEx.Message, dbEx);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region TEXT TEMPLATE METHODS
        public ApiResponse<Text_Templates> AddTextTemplate(long UserID, Text_TemplatesModel model)
        {
            return AddUpdateTextTemplate(UserID, model);
        }

        public ApiResponse<Text_Templates> UpdateTextTemplate(long UserID, Text_TemplatesModel model, long textTemplateId)
        {
            return AddUpdateTextTemplate(UserID, model, textTemplateId: textTemplateId);
        }

        private ApiResponse<Text_Templates> AddUpdateTextTemplate(long UserID, Text_TemplatesModel model, long textTemplateId = 0)
        {
            var result = new ApiResponse<Text_Templates>();
            try
            {
                var employer = _employersRepositoryService.Get(x => x.UserID == UserID);
                if (employer != null)
                {
                    var text_Template = new Text_Templates();
                    if (textTemplateId > 0)
                    {
                        text_Template = _text_TemplatesRepositoryService.Get(x => x.ID == textTemplateId && x.EmployerID == employer.ID);
                    }

                    text_Template.Text_Content = model.Text_Content;
                    text_Template.Name = model.Name;
                    text_Template.EmployerID = employer.ID;

                    if (text_Template.ID < 1)
                        text_Template = _text_TemplatesRepositoryService.Add(text_Template);
                    else
                        text_Template = _text_TemplatesRepositoryService.Update(text_Template);

                    result.Data = text_Template;
                }
            }
            catch (DbUpdateException dbEx)
            {
                result.AddError(dbEx.Message);
                result.Data = null;
                _log.Error(dbEx.Message, dbEx);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Data = null;
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        public ApiResponse RemoveTextTemplate(long UserID, long textTemplateId)
        {
            var result = new ApiResponse();
            try
            {
                var employer = _employersRepositoryService.Get(x => x.UserID == UserID);
                if (employer != null)
                {
                    _text_TemplatesRepositoryService.Delete(x => x.ID == textTemplateId && x.EmployerID == employer.ID);
                }
                else
                    result.AddError("Bad request!");
            }
            catch (DbUpdateException dbEx)
            {
                result.AddError(dbEx.Message);
                _log.Error(dbEx.Message, dbEx);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _log.Error(ex.Message, ex);
            }
            return result;
        }

        #endregion
    }
}
