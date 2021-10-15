using NServiceBus;
using System;
using System.Threading.Tasks;
using SagaScatterGather.Shared.Commands;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Saga.Endpoint
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "SagaScatterGather.Saga.Endpoint";

            var endpointConfiguration = new EndpointConfiguration("SagaScatterGather.Saga.Endpoint");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.Routing().RouteToEndpoint(messageType: typeof(Vendor1QuoteRequest), destination: "SagaScatterGather.Vendor1.Endpoint");
            transport.Routing().RouteToEndpoint(messageType: typeof(Vendor2QuoteRequest), destination: "SagaScatterGather.Vendor2.Endpoint");
            transport.Routing().RouteToEndpoint(messageType: typeof(Vendor3QuoteRequest), destination: "SagaScatterGather.Vendor3.Endpoint");

            var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            await Task.Delay(3000);

            await endpointInstance.SendLocal(new GetQuote {QuoteId = Guid.NewGuid()});

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
