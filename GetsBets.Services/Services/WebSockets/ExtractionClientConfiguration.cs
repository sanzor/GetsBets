using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public class ExtractionClientConfiguration:IExtractionClientConfiguration
    {
        public int ThrottleMilliseconds { get; set; }
        public int SendNumberAfterSeconds { get; set; }

       

        public static ExtractionClientConfiguration GetFromConfiguration(IConfiguration configuration)
        {
            var throttle = configuration.GetSection("extractionClient:throttleMilliseconds").Get<int>();
            var sendNumberAfterSeconds = configuration.GetSection("extractionClient:sendNumbersAfterSeconds").Get<int>();
            return new ExtractionClientConfiguration { ThrottleMilliseconds= throttle, SendNumberAfterSeconds= sendNumberAfterSeconds };


        }
    }
}
