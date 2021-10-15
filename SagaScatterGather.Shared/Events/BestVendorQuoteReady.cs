using System;
using System.Collections.Generic;
using NServiceBus;

namespace SagaScatterGather.Shared.Events
{
    public class BestVendorQuoteReady : IEvent
    {
        public Guid QuoteId { get; set; }
        public decimal BestQuote { get; set; }
        public List<string> ExcludedVendors { get; set; } = new List<string>();
    }
}
