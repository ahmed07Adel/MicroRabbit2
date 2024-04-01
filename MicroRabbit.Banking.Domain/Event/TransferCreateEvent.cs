using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Domain.Event
{
    public class TransferCreateEvent : MicroRabbit.Domain.Core.Events.Event
    {
        public int From { get; set; }
            
        public int To { get; set; }
        public decimal Amount { get; set; }
        public TransferCreateEvent(int from, int to, decimal amount)
        {
            From = from;
            To = to;
            Amount = amount;    
        }
    }
}
