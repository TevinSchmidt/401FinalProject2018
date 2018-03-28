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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Messages.ServiceBusRequest.CompanyReviews;
using Messages.ServiceBusRequest.CompanyReviews.Requests;
using Messages.DataTypes.Database.CompanyReview;
using System.Collections.Generic;
namespace EchoService.Handlers
{
    class SearchReviewsEventHandler : IHandleMessages<CompanyReviewSearchRequest>
    {

        static ILog log = LogManager.GetLogger<CompanyReviewSearchRequest>();

        public Task Handle(CompanyReviewSearchRequest message, IMessageHandlerContext context)
        {
            //TODO: fix the next line
            string url = "NEED TO PUT THE URL HERE";
            HttpClient httpClient = new HttpClient();
            try
            {
                string company = "{\"companyName\":\"" + message.companyName + "\"}";
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                System.Threading.Tasks.Task<HttpResponseMessage> wcfresponse = httpClient.PostAsync(url, new StringContent(company, Encoding.UTF8, "application/json"));
                List<Review> reviews = JsonConvert.DeserializeObject<List<Review>>(wcfresponse.ToString());
                return context.Reply(new CompanyReviewResponse(true, "Reviews for company name : " + message.companyName, reviews));
            }
            catch (HttpRequestException e)
            {
                //TODO: Error Message
            }
            return context.Reply(new CompanyReviewResponse(false, "FAILED to acomplish task with messageID: " + context.MessageId, null));
        }
    }
}
