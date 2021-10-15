using System;
using NServiceBus;

namespace SagaScatterGather.Shared.Messages
{
    public class Vendor1QuoteResponse : IMessage
    {
        public Guid QuoteId { get; set; }
        public decimal QuoteAmount { get; set; }
    }
}
