using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Messages.DataTypes.Database.CompanyReview;

namespace Messages.NServiceBus.Events
{
    [Serializable]
    public class ReviewEvent : IEvent
    {
        public Review review { get; set; }
        public string companyname { get; set; }
        public bool isNewReview { get; set; }
    }
}
