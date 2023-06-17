using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediatr;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        public RabbitMQBus(IMediator mediator)
        {
            _mediatr = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>(); 
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            throw new NotImplementedException();
        }
        public void Publish<T>(T @event) where T : Event
        {
            //publish to our Q
            var factory = new ConnectionFactory()
            {
                HostName="localhost"
            };
            //save our connection
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //we can grab name of the event by knowing the type of uisng relection
                var eventname = @event.GetType().Name;

                //declare q in our rabbit MQ server, iam going to use channel and Q
                channel.QueueDeclare(eventname,false,false,false,null);

                //message and serialize Object
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message); 

                //use channel to publish message
                channel.BasicPublish("",eventname,null, body);

            }


        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            //extract event name
            var eventname = typeof(T).Name;
            var handlertype = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventname))
            {
                _handlers.Add(eventname, new List<Type>());
            }
            if (_handlers[eventname].Any(a => a.GetType() == handlertype))
            {
                throw new ArgumentException($"Handler Type {handlertype.Name} is Alrady registered For {eventname}", nameof(handlertype));
            }
            _handlers[eventname].Add(handlertype);
            //after subscripe , Start Consuming Messages Here
            StartBasicConsumer<T>();
        }

        private void StartBasicConsumer<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName="localhost",
                //For Async Consumer
                DispatchConsumersAsync = true
            };
            //create connection from factory
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var eventname = typeof(T).Name;

            //declare Queue
            channel.QueueDeclare(eventname, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Recieved;

            channel.BasicConsume(eventname, true, consumer);
        }


        // 3shan low 3ndy aktr mn Queue
        //create delegate
        private async Task Consumer_Recieved(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                // determne which event will subscribe
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception)
            {

                throw;
            }
        }


        // handle any cause in event bus 
        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                var subscribtions = _handlers[eventName];
                foreach (var item in subscribtions)
                {
                    var handler = Activator.CreateInstance(item);
                    if (handler == null)
                    {
                        continue;
                    }
                    var eventType = _eventTypes.SingleOrDefault(a => a.Name == eventName);
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                }
            }
        }
    }
}
