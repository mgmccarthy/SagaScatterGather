using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using NServiceBus;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Commands;
using SagaScatterGather.Shared.Events;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Saga.Endpoint
{
    public class VendorQuoteSaga : Saga<VendorQuoteSaga.SagaData>,
        IAmStartedByMessages<GetQuote>,
        IHandleMessages<Vendor1QuoteResponse>,
        IHandleMessages<Vendor2QuoteResponse>,
        IHandleMessages<Vendor3QuoteResponse>,
        IHandleTimeouts<VendorQuoteSaga.TimeoutState>
    {
        private static readonly ILog Log = LogManager.GetLogger<VendorQuoteSaga>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<GetQuote>(message => message.QuoteId).ToSaga(sagaData => sagaData.QuoteId);
        }

        public async Task Handle(GetQuote message, IMessageHandlerContext context)
        {
            Log.Info("Handling GetQuote");
            await context.Send(new Vendor1QuoteRequest { QuoteId = message.QuoteId });
            await context.Send(new Vendor2QuoteRequest { QuoteId = message.QuoteId });
            await context.Send(new Vendor3QuoteRequest { QuoteId = message.QuoteId });
            
            //8 second SLA for the user to get their quote results
            //await RequestTimeout<TimeoutState>(context, TimeSpan.FromSeconds(8));
            await RequestTimeout<TimeoutState>(context, TimeSpan.FromMinutes(1));
        }

        public async Task Handle(Vendor1QuoteResponse message, IMessageHandlerContext context)
        {
            Log.Info($"Handling Vendor1QuoteResponse with quote value of: {message.QuoteAmount}");
            
            Data.VendorQuotes.Add("Vendor1Quote", message.QuoteAmount);
            
            await CheckForAllVendorQuotesReceived(context);
        }

        public async Task Handle(Vendor2QuoteResponse message, IMessageHandlerContext context)
        {
            Log.Info($"Handling Vendor2QuoteResponse with quote value of: {message.QuoteAmount}");
            
            Data.VendorQuotes.Add("Vendor2Quote", message.QuoteAmount);

            await CheckForAllVendorQuotesReceived(context);
        }

        public async Task Handle(Vendor3QuoteResponse message, IMessageHandlerContext context)
        {
            Log.Info($"Handling Vendor3QuoteResponse with quote value of: {message.QuoteAmount}");
            
            Data.VendorQuotes.Add("Vendor3Quote", message.QuoteAmount);

            await CheckForAllVendorQuotesReceived(context);
        }

        private async Task CheckForAllVendorQuotesReceived(IMessageHandlerContext context)
        {
            //Have all vendors returned a quote
            if (Data.VendorQuotes.Keys.Contains("Vendor1Quote") && Data.VendorQuotes.Keys.Contains("Vendor2Quote") && Data.VendorQuotes.Keys.Contains("Vendor3Quote"))
            {
                await context.Publish(new BestVendorQuoteReady
                {
                    QuoteId = Data.QuoteId,
                    BestQuote = Data.VendorQuotes.Values.Min()
                });

                MarkAsComplete();
            }
        }

        public async Task Timeout(TimeoutState state, IMessageHandlerContext context)
        {
            //check one more time for all vendor quotes received
            await CheckForAllVendorQuotesReceived(context);

            //publish sla breached event
            await context.Publish(new VendorQuoteSagaSlaBreached { QuoteId = Data.QuoteId });

            MarkAsComplete();
        }

        public class SagaData : ContainSagaData
        {
            public Guid QuoteId { get; set; }

            public Dictionary<string, decimal> VendorQuotes { get; set; } = new Dictionary<string, decimal>();
            //public decimal Vendor1Quote { get; set; }
            //public decimal Vendor2Quote { get; set; }
            //public decimal Vendor3Quote { get; set; }
        }

        public class TimeoutState { }
    }
}