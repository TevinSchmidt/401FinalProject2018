

using Messages;
using Messages.NServiceBus.Commands;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Chat;
using Messages.ServiceBusRequest.Chat.Requests;

using NServiceBus;

using System;
using System.Collections.Generic;
using System.Net;

namespace AuthenticationService.Communication
{
    partial class ClientConnection
    {


        /// <summary>
        /// Listens for the client to specify which task it is requesting from the authentication service
        /// </summary>
        /// <param name="request">Contains Information regarding the task being requested</param>
        /// <returns>A response message</returns>
        private ServiceBusResponse chatRequest(ChatServiceRequest request)
        {
            switch (request.requestType)
            {
                case (ChatRequest.getChatContacts):
                    return getContacts((GetChatContactsRequest)request);
                case (ChatRequest.getChatHistory):
                    return getHistory((GetChatHistoryRequest)request);
                case (ChatRequest.sendMessage):
                    return sendMessage((SendMessageRequest)request);
                default:
                    return new ServiceBusResponse(false, "Error: Invalid request. Request received was:" + request.requestType.ToString());
            }
        }


        private ServiceBusResponse getContacts(GetChatContactsRequest request)
        {
            if (authenticated == false)
            {
                return new ServiceBusResponse(false, "Error: You must be logged in to use the getContacts functionality.");
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Chat");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }


        private ServiceBusResponse getHistory(GetChatHistoryRequest request)
        {
            if (authenticated == false)
            {
                return new ServiceBusResponse(false, "Error: You must be logged in to use the get History functionality.");
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Chat");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }


        private ServiceBusResponse sendMessage(SendMessageRequest request)
        {
            if (authenticated == false)
            {
                return new ServiceBusResponse(false, "Error: You must be logged in to use the send message functionality.");
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Chat");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
