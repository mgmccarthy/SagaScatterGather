using System;
using NServiceBus;

namespace SagaScatterGather.Shared.Messages
{
    public class Vendor3QuoteRequest : IMessage
    {
        public Guid QuoteId { get; set; }
    }
}
