using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.CompanyReviews
{
    [Serializable]
    public class CompanyReviewServiceRequest : ServiceBusRequest
    {
        public CompanyReviewServiceRequest(CompanyReviewRequest requestType)
            : base(Service.CompanyReviews)
        {
            this.requestType = requestType;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the Company Directory Service
        /// </summary>
        public CompanyReviewRequest requestType;
    }

    public enum CompanyReviewRequest { SearchReviews, AddReview };
}
