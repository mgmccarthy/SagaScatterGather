using System;
using System.Collections.Generic;
using System.Text;
using NServiceBus;

namespace SagaScatterGather.Shared.Events
{
    public class BestVendorQuoteReady : IEvent
    {
        public Guid QuoteId { get; set; }
        public decimal BestQuote { get; set; }
    }
}
