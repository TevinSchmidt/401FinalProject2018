using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.NServiceBus.Events;
using System.Net.Http;
using Messages.ServiceBusRequest;
using NServiceBus;
using NServiceBus.Logging;
using Newtonsoft.Json;

using Messages.ServiceBusRequest.CompanyReviews;
using Messages.ServiceBusRequest.CompanyReviews.Requests;
using Messages.DataTypes.Database.CompanyReview;

namespace EchoService.Handlers
{
    class SearchReviewsEventHandler : IHandleMessages<CompanyReviewSearchRequest>
    {
        private class ReviewList {
            [JsonProperty("reviews")]
            public List<Review> allReviews { get; set; }
            public int count { get; set; }
        } 

        static ILog log = LogManager.GetLogger<CompanyReviewSearchRequest>();

        public Task Handle(CompanyReviewSearchRequest message, IMessageHandlerContext context)
        {
            //TODO: fix the next line
            string url = "http://35.192.218.206/Home/GetCompanyReview/";
            HttpClient httpClient = new HttpClient();

            try
            {
                string company = "{\"companyName\":\"" + message.companyName + "\"}";
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage wcfresponse = httpClient.GetAsync(url + company).GetAwaiter().GetResult();
                string test = wcfresponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                
                ReviewList reviews = JsonConvert.DeserializeObject<ReviewList>(test);
                if (reviews.count <= 0)
                {
                    return context.Reply(new CompanyReviewResponse(true, "Reviews for company name : " + message.companyName, reviews.allReviews));
                }
                
            }
            catch (HttpRequestException)
            {
                //TODO: Error Message
            }
            catch (Exception) { }
            return context.Reply(new CompanyReviewResponse(false, "FAILED to acomplish task with messageID: " + context.MessageId, null));
        }
    }
}
