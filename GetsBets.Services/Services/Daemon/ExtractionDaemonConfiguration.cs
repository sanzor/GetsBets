
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public class ExtractionDaemonConfiguration:IExtractionDaemonConfiguration
    {
        private const string DaemonKey = "daemon";
        public ushort RunExtractionAfterMinutes { get; init; }
        public static ExtractionDaemonConfiguration GetFromConfiguration(IConfiguration configuration)
        {
            var config = configuration.GetSection(DaemonKey).Get<ExtractionDaemonConfiguration>();
            return config;

        }
    }
}
