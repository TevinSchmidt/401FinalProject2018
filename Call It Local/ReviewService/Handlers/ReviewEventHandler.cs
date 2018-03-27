﻿using Messages.NServiceBus.Events;
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
    /// <summary>
    /// This is the handler class for the reverse echo. 
    /// This class is created and its methods called by the NServiceBus framework
    /// </summary>
    public class ReviewEventHandler : IHandleMessages<AddCompanyReviewRequest>
    {

        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<AddCompanyReviewRequest>();

        /// <summary>
        /// 
        /// This method will be called by the NServiceBus framework when an event of type "ReviewEvent" is published.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context"></param>
        /// <returns>Nothing</returns>
        public Task Handle(AddCompanyReviewRequest message, IMessageHandlerContext context)
        {
            //TODO: fix the next line
            string url = "NEED TO PUT THE URL HERE";
            HttpClient httpClient = new HttpClient();
                try
                {
                    string json = JsonConvert.SerializeObject(message.review);
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    System.Threading.Tasks.Task<HttpResponseMessage> wcfresponse = httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                    return Task.CompletedTask;
                }
            catch(HttpRequestException e)
                {
                    //TODO: Error Message
                }
            return context.Reply(new CompanyReviewResponse(false, "FAILED to acomplish task with messageID: " + context.MessageId, null));
        }
    }
}
