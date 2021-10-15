using NServiceBus;
using System;
using System.Threading.Tasks;

namespace SagaScatterGather.Saga.Endpoint
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "SagaScatterGather.Saga.Endpoint";

            var endpointConfiguration = new EndpointConfiguration("SagaScatterGather.Saga.Endpoint");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
