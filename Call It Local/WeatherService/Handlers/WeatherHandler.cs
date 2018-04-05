

using Messages.NServiceBus.Events;
using NServiceBus;
using NServiceBus.Logging;
using Messages.ServiceBusRequest.Weather.Requests;
using Messages.ServiceBusRequest.Weather.Response;
using Messages.DataTypes;
using System.Threading.Tasks;
using Messages.NServiceBus.Events;
using System.Net.Http;
using Messages.ServiceBusRequest;
using NServiceBus;
using NServiceBus.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
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
    public class WeatherHandler : IHandleMessages<WeatherNeededRequest>
    {
        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<WeatherNeededRequest>();

        /// <summary>
        /// Handler that will return the data from the JSON of the website for the weather
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context"></param>
        /// <returns>Nothing</returns>
        public Task Handle(WeatherNeededRequest message, IMessageHandlerContext context)
        {
            string url = "";
            HttpClient httpClient = new HttpClient();
            try
            {
                string json = message.location;
                //Get information regarding the key, using the location provided
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage wcfresponse = httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
                string jsonWithKEY = wcfresponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                //Get weather data based on the key
                string extractedKey = jsonWithKEY;
                url = ""; //forecast data url
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                wcfresponse = httpClient.PostAsync(url, new StringContent(extractedKey, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
                string resultingData = wcfresponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                string temp;
                string cloudCover;
                string realFeel;
                //remove data from resultingData that is required
                WeatherData WD = new WeatherData(temp, cloudCover, realFeel);
                return context.Reply(new WeatherNeededResponse(true, "Weather for: " + message.location, WD));
            }
            catch
            {

            }

            return context.Reply(new ServiceBusResponse(false, "FAILED to add review"));
        }
    }
}
