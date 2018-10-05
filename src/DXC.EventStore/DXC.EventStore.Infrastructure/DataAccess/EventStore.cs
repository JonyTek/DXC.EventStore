using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace DXC.EventStore.Infrastructure.DataAccess
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreConnection connection;

        public EventStore(IEventStoreConnection connection)
        {
            this.connection = connection;
        }

        public async Task AppendEvent<TAggregateId>(DomainEventBase<TAggregateId> evnt)
            where TAggregateId : IAggregateId
        {
            var eventData = new EventData(
                evnt.EventId, evnt.GetType().AssemblyQualifiedName, true, Serialize(evnt),
                Encoding.UTF8.GetBytes("{}"));

            var writeResult =
                await connection.AppendToStreamAsync(
                    evnt.AggregateId.AsString,
                    evnt.AggregateVersion - 1,
                    eventData);
        }

        public async Task<IEnumerable<Event<TAggregateId>>> ReadEventsAsync<TAggregateId>(TAggregateId id)
            where TAggregateId : IAggregateId
        {
            StreamEventsSlice currentSlice;
            var events = new List<Event<TAggregateId>>();
            long nextSliceStart = StreamPosition.Start;

            do
            {
                currentSlice = await connection.ReadStreamEventsForwardAsync(id.AsString, nextSliceStart, 200, false);
                if (currentSlice.Status != SliceReadStatus.Success)
                {
                    throw new Exception($"Aggregate {id.AsString} not found");
                }

                nextSliceStart = currentSlice.NextEventNumber;

                foreach (var resolvedEvent in currentSlice.Events)
                {
                    events.Add(new Event<TAggregateId>(
                        Deserialize<TAggregateId>(resolvedEvent.Event.EventType, resolvedEvent.Event.Data),
                        resolvedEvent.Event.EventNumber));
                }

            } while (!currentSlice.IsEndOfStream);

            return events;
        }

        private static DomainEventBase<TAggregateId> Deserialize<TAggregateId>(string eventType, byte[] data)
            where TAggregateId : IAggregateId
        {
            var settings = new JsonSerializerSettings {ContractResolver = new PrivateSetterContractResolver()};
            var eventJson = Encoding.UTF8.GetString(data);
            var evnt = JsonConvert.DeserializeObject(eventJson, Type.GetType(eventType), settings);

            return (DomainEventBase<TAggregateId>) evnt;
        }

        private static byte[] Serialize<TAggregateId>(DomainEventBase<TAggregateId> evnt)
            where TAggregateId : IAggregateId
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt));
        }
    }
}