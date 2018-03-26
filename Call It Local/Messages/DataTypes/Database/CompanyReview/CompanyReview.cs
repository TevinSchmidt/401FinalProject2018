using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages.DataTypes.Database.CompanyReview
{
    [Serializable]
    public partial class CompanyReview
    {
        public CompanyReview(string companyName, string review, string stars, string timestamp, string username)
        {
            this.companyName = companyName;
            this.review = review;
            this.stars = stars;
            this.timestamp = timestamp;
            this.username = username;
        }
    }

    public partial class CompanyReview
    {
        /// <summary>
        /// The name of the company
        /// </summary>
        public String companyName { get; set; } = null;

        /// <summary>
        /// The review of the company
        /// </summary>
        public String review { get; set; } = null;

        /// <summary>
        /// The stars for the review
        /// </summary>
        public String stars { get; set; } = null;

        /// <summary>
        /// A timestamp of the review
        /// </summary>
        public String timestamp { get; set; } = null;


        /// <summary>
        /// The username of the reviewer
        /// </summary>
        public String username { get; set; } = null;


    }
}
