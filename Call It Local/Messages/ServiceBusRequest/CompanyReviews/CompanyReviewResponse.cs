using Messages.DataTypes.Database.CompanyReview;
using System;
using Messages.ServiceBusRequest.CompanyReviews;
using System.Collections.Generic;

namespace Messages.ServiceBusRequest.CompanyReviews
{
    [Serializable]
    public class CompanyReviewResponse : ServiceBusResponse
    {
        public CompanyReviewResponse(bool result, string response, List<CompanyReview> reviews)
            : base(result, response)
        {
            this.reviews = reviews;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the Company Directory Service
        /// </summary>
        public List<CompanyReview> reviews;
    }
}
