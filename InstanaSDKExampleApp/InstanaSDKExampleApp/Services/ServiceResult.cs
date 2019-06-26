using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstanaSDKExampleApp.Services
{
    public class ServiceResult<TResult>
    {
        public int StatusCode
        {
            get;
            set;
        }

        public Exception CallException
        {
            get;
            set;
        }

        public TResult Result
        {
            get;
            set;
        }

        public bool WasSuccessful
        {
            get
            {
                return CallException == null && StatusCode == 200;
            }
        }
    }
}
