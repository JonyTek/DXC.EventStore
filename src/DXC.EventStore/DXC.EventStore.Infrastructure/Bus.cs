using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using DXC.EventStore.Contracts;

namespace DXC.EventStore.Infrastructure
{
    public class Bus : IBus
    {
        private readonly IHttpClientFactory httpClientFactory;

        public Bus(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task Publish<TMessage>(TMessage message)
            where TMessage : AbstractMessage
        {
            var client = httpClientFactory.CreateClient();
            var result = await client.PostAsync(message.Destination,
                new ObjectContent<TMessage>(message, new JsonMediaTypeFormatter()));

            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.Accepted)
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }
        }
    }
}