using NServiceBus;
using System;
using System.Threading.Tasks;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Commands;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Saga.Endpoint
{
    class Program
    {
        static ILog log = LogManager.GetLogger<Program>();

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

            await RunLoop(endpointInstance).ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);
        }

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                log.Info("Press 'P' to get a Quote, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        await endpointInstance.SendLocal(new GetQuote { QuoteId = Guid.NewGuid() });
                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}
