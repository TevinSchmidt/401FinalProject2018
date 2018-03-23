﻿using DirectoryService.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using NServiceBus;
using NServiceBus.Logging;



namespace DirectoryService.Handlers
{
    class GetCompanyInfoHandler : IHandleMessages<GetCompanyInfoRequest>
    {
        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<GetCompanyInfoRequest>();

        /// <summary>
        /// Saves the echo to the database, reverses the data, and returns it back to the calling endpoint.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context">Used to access information regarding the endpoints used for this handle</param>
        /// <returns>The response to be sent back to the calling process</returns>
        public Task Handle(GetCompanyInfoRequest message, IMessageHandlerContext context)
        {
            //Save the echo to the database
            GetCompanyInfoResponse resp = DirectoryServiceDatabase.getInstance().getCompany(message);

            // Do something here and return a COMPANYSEARCHRESPONSE

            //The context is used to give a reply back to the endpoint that sent the request
            return context.Reply(resp);
        }
    }
}
