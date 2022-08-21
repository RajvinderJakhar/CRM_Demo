using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Constants
{
    public class RemoteConstants
    {
        /// <summary>
        /// JWT Token type
        /// </summary>
        public const string TokenType = "Bearer";
        /// <summary>
        /// Access to auth service
        /// </summary>
        public const string AuthAPIRole = "AccessAuthService";
        /// <summary>
        /// Access to employer service
        /// </summary>
        public const string EmployerAPIRole = "AccessEmployerService";
        /// <summary>
        /// Access to candidate service
        /// </summary>
        public const string CandidateAPIRole = "AccessCandidateService";

        /// <summary>
        /// Auth api base path
        /// </summary>
        public const string AuthServicePath = "/auth/";
        /// <summary>
        /// Employer remote api base path
        /// </summary>
        public const string EmployerServicePath = "/employer/remote/emp-commu/";
        /// <summary>
        /// Candidate api base path
        /// </summary>
        public const string CandidateServicePath = "/employer/";
    }
}
