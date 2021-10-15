using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Vendor1.Endpoint
{
    public class QuoteRequestHandler : IHandleMessages<Vendor1QuoteRequest>
    {
        private static readonly ILog Log = LogManager.GetLogger<QuoteRequestHandler>();

        public async Task Handle(Vendor1QuoteRequest message, IMessageHandlerContext context)
        {
            Log.Info("Handling Vendor1QuoteRequest");
            
            //await Task.Delay(3000);
            await Task.Delay(10000); //TOO SLOW!
            
            await context.Reply(new Vendor1QuoteResponse { QuoteId = message.QuoteId, QuoteAmount = 200 });
        }
    }
}
