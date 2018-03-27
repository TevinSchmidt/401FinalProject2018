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
    public class AddReviewEvent : IEvent
    {
        public Review review { get; set; }
       
    }
}
