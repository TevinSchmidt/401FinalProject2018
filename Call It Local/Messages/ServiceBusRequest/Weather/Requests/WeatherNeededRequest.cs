using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.Weather.Requests
{
    public class WeatherNeededRequest : ServiceBusRequest
    {
        /// <summary>
        /// constructor for the request
        /// </summary>
        /// <param name="location"></param>
        public WeatherNeededRequest(string location) : base(Service.Weather)
        {
            this.location = location;
        }

        /// <summary>
        /// Location that is used to find the weather data for
        /// </summary>
        public string location;
    }
}
