using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.CompanyReviews
{
    [Serializable]
    public class CompanySearchRequest : ServiceBusRequest
    {
        public CompanySearchRequest(string companyName)
            : base(Service.CompanyReviews)
        {
            this.companyName = companyName;
        }

        /// <summary>
        /// Information used to search the database for companies
        /// </summary>
        public string companyName;
    }
}
