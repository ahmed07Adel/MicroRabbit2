using MicroRabbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Domain.Core.Bus
{
    public interface IEventBus
    {
        //Sent To Bus
        Task SendCommand<T> (T command) where T : Command;        

        // publish any type of Event
        void Publish<T> (T @event) where T : Events.Event;
        void Subscribe<T, TH>()
            where T : Events.Event
            where TH : IEventHandler<T>;
    }
}
