using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public interface IBroadcastService<T>
    {
        EitherAsync<Error, Unit> PublishMessageAsync<T>(string channel,T message);
    }
}
