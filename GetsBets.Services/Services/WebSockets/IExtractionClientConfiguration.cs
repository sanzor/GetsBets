using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    internal interface IExtractionClientConfiguration
    {
        public int ThrottleMilliseconds { get; }
        public int SendNumberAfterSeconds { get; }
    }
}
