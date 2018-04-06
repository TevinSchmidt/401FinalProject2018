using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NServiceBus;
using Messages.NServiceBus.Commands;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Weather;
using Messages.ServiceBusRequest.Weather.Requests;

namespace AuthenticationService.Communication
{
    partial class ClientConnection
    {

        private ServiceBusResponse weatherRequest(WeatherNeededRequest request) {

            if (authenticated == false)
            {
                return new ServiceBusResponse(false, "Error: You must be logged in to use the weather request.");
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("WeatherService");


            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult(); ;
        }
    }
}
