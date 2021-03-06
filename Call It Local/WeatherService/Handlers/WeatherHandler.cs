﻿using Newtonsoft.Json.Linq;

using Messages.NServiceBus.Events;
using NServiceBus;
using NServiceBus.Logging;
using Messages.ServiceBusRequest.Weather.Requests;
using Messages.ServiceBusRequest.Weather.Response;
using Messages.DataTypes;
using System.Threading.Tasks;

using System.Net.Http;
using Messages.ServiceBusRequest;

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
            string apiKey = "s4FAawn3XfbbA2rsgGtqyFKBWPZ3opvc";
            string city = message.location;
            string url = "https://dataservice.accuweather.com/locations/v1/cities/search?apikey=" +
                apiKey + "&q=" + city;
            HttpClient httpClient = new HttpClient();
            try
            {
                
                //Get information regarding the key, using the location provided
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage wcfresponse = httpClient.GetAsync(url).GetAwaiter().GetResult();
                string response = wcfresponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                
                JArray json = JArray.Parse(response);

                if(!json.HasValues)
                    return context.Reply(new WeatherNeededResponse(false, "FAILED to add review", null));


                //Get weather data based on the key
                string extractedKey = json[0].Value<string>("Key");
                url = "https://dataservice.accuweather.com/currentconditions/v1/"
                    + extractedKey + "?apikey=" + apiKey + "&details=true"; //forecast data url
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                wcfresponse = httpClient.GetAsync(url).GetAwaiter().GetResult();
                string resultingData = wcfresponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var weatherData = JArray.Parse(resultingData);
                string temp = weatherData[0]["Temperature"]["Metric"].Value<string>("Value");
                string tempUnit = weatherData[0]["Temperature"]["Metric"].Value<string>("Unit");
                string weatherText = weatherData[0].Value<string>("WeatherText");
                string weatherIcon = weatherData[0].Value<string>("WeatherIcon");
                string realTemp = weatherData[0]["RealFeelTemperature"]["Metric"].Value<string>("Value");
                string realTempUnit = weatherData[0]["RealFeelTemperature"]["Metric"].Value<string>("Unit");
                //remove data from resultingData that is required
                WeatherData WD = new WeatherData(temp, tempUnit, realTemp, realTempUnit, weatherText, weatherIcon);
                return context.Reply(new WeatherNeededResponse(true, "Weather for: " + message.location, WD));
            }
            catch
            {
                return context.Reply(new ServiceBusResponse(false, "FAILED to find weather"));
            }

            return context.Reply(new ServiceBusResponse(false, "FAILED to add review"));
        }
    }
}
