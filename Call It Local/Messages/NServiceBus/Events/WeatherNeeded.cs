using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.NServiceBus.Commands;
using Messages.DataTypes;
using NServiceBus;

namespace Messages.NServiceBus.Events
{
    public class WeatherNeeded : IEvent
    {
        public WeatherNeeded()
        {

        }
    }
}
