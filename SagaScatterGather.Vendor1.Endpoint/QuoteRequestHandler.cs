using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Vendor1.Endpoint
{
    public class QuoteRequestHandler : IHandleMessages<Vendor1QuoteRequest>
    {
        private static readonly ILog Log = LogManager.GetLogger<QuoteRequestHandler>();

        public Task Handle(Vendor1QuoteRequest message, IMessageHandlerContext context)
        {
            Log.Info("Handling Vendor1QuoteRequest");
            return context.Reply(new Vendor1QuoteResponse { QuoteId = message.QuoteId, QuoteAmount = 200 });
        }
    }
}
