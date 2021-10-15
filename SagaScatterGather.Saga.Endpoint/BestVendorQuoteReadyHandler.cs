using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Events;

namespace SagaScatterGather.Saga.Endpoint
{
    public class BestVendorQuoteReadyHandler : IHandleMessages<BestVendorQuoteReady>
    {
        private static readonly ILog Log = LogManager.GetLogger<BestVendorQuoteReadyHandler>();

        public Task Handle(BestVendorQuoteReady message, IMessageHandlerContext context)
        {
            Log.Info($"Best Vendor Quote for Quote Id: {message.QuoteId} is {message.BestQuote}");
            return Task.CompletedTask;
        }
    }
}
