using System;
using NServiceBus;

namespace SagaScatterGather.Shared.Events
{
    public class VendorQuoteSlaBreached : IEvent
    {
        public Guid QuoteId { get; set; }
    }
}