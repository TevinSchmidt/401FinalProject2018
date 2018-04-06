using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.DataTypes
{
    [Serializable]
    public class WeatherData
    {
        public WeatherData(string temp, string tempUnit, string weatherText, string iconNum)
        {
            this.temp = temp;
            this.tempUnit = tempUnit;
            this.weatherText = weatherText;
            this.iconNum = iconNum;
        }
        public string temp;
        public string weatherText;
        public string tempUnit;
        public string iconNum;
    }
}
