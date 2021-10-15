using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Vendor3.Endpoint
{
    public class QuoteRequestHandler : IHandleMessages<Vendor3QuoteRequest>
    {
        private static readonly ILog Log = LogManager.GetLogger<QuoteRequestHandler>();

        public async Task Handle(Vendor3QuoteRequest message, IMessageHandlerContext context)
        {
            Log.Info("Handling Vendor3QuoteRequest");
            await Task.Delay(1000); //simulate RPC
            await context.Reply(new Vendor3QuoteResponse { QuoteId = message.QuoteId, QuoteAmount = 400 });
        }
    }
}
