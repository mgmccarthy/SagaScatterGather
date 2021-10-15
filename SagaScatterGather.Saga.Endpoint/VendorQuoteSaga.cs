using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using SagaScatterGather.Shared.Commands;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Saga.Endpoint
{
    public class VendorQuoteSaga : Saga<VendorQuoteSaga.SagaData>,
        IAmStartedByMessages<GetQuote>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<GetQuote>(message => message.QuoteId).ToSaga(sagaData => sagaData.QuoteId);
        }

        public async Task Handle(GetQuote message, IMessageHandlerContext context)
        {
            await context.Send(new Vendor1QuoteRequest { QuoteId = message.QuoteId });
            await context.Send(new Vendor2QuoteRequest { QuoteId = message.QuoteId });
            await context.Send(new Vendor3QuoteRequest { QuoteId = message.QuoteId });
        }

        public class SagaData : ContainSagaData
        {
            public Guid QuoteId { get; set; }
        }
    }
}
