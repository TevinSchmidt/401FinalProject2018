using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.DataTypes;

namespace Messages.ServiceBusRequest.Weather.Response
{
    [Serializable]
    public class WeatherNeededResponse : ServiceBusResponse
    {
        public WeatherNeededResponse(bool result, string response, WeatherData WD) : base (result, response)
        {
            this.weatherData = WD;
        }

        /// <summary>
        /// data required to show weather data
        /// </summary>
        public WeatherData weatherData;
    }
}
