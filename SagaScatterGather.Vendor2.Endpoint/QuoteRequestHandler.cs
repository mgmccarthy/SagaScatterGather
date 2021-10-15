using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Vendor2.Endpoint
{
    public class QuoteRequestHandler : IHandleMessages<Vendor2QuoteRequest>
    {
        private static readonly ILog Log = LogManager.GetLogger<QuoteRequestHandler>();

        public async Task Handle(Vendor2QuoteRequest message, IMessageHandlerContext context)
        {
            Log.Info("Handling Vendor2QuoteRequest");
            await Task.Delay(2000); //simulate RPC
            await context.Reply(new Vendor2QuoteResponse { QuoteId = message.QuoteId, QuoteAmount = 300 });
        }
    }
}
