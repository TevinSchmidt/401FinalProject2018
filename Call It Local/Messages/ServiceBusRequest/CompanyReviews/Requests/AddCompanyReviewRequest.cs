using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.DataTypes.Database.CompanyReview;

namespace Messages.ServiceBusRequest.CompanyReviews.Requests
{
    [Serializable]
    public class AddCompanyReviewRequest : CompanyReviewServiceRequest
    {
        public AddCompanyReviewRequest(Review review)
            : base(CompanyReviewRequest.AddReview)
        {
            this.review = review;
        }

        /// <summary>
        /// The data the client sent to be stored
        /// </summary>
        public Review review;
    }
}
