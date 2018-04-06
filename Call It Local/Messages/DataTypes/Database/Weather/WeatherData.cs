using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.DataTypes
{
    public class WeatherData
    {
        public WeatherData(string temp, string cloudcover, string realFeel)
        {
            this.temp = temp;
            this.cloudcover = cloudcover;
            this.realFeel = realFeel;
        }
        public string temp;
        public string cloudcover;
        public string realFeel;
    }
}
