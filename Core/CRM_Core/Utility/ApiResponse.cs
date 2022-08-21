using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Utility
{
    public class ApiResponse<T> : ApiResponse where T : class, new() 
    {
        public T Data { get; set; } = new T();
    }

    public class ApiResponse
    {
        public IList<string> Errors { get; set; } = new List<string>();
        public string ErrorsStr
        {
            get
            {
                return String.Join(Environment.NewLine, this.Errors);
            }
        }

        public bool Success
        {
            get { return this.Errors.Count == 0; }
        }

        public void AddError(string error)
        {
            this.Errors.Add(error);
        }
    }
}
