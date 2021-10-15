using System;
using NServiceBus;

namespace SagaScatterGather.Shared.Events
{
    public class VendorQuoteSagaSlaBreached : IEvent
    {
        public Guid QuoteId { get; set; }
    }
}
