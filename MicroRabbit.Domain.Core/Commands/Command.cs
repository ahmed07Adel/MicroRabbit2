using MicroRabbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Domain.Core.Commands
{
    public abstract class Command : Message
    {
        //sending message across our bus
        public DateTime TimeStamp { get; set; }
        public Command()
        {
            TimeStamp = DateTime.Now;   
        }
    }
}
