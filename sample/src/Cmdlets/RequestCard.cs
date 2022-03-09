using AutomationIoC.Sample.Models;
using Microsoft.Extensions.Logging;
using System.Management.Automation;

namespace AutomationIoC.Sample.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Request, "Card")]
    public class RequestCard : IoCShell
    {
        [AutomationDependency]
        private readonly Deck cardDeck;

        [AutomationDependency]
        private readonly ILogger<RequestCard> logger;

        public RequestCard()
        {
        }

        public RequestCard(Deck cardDeck, ILogger<RequestCard> logger)
        {
            this.cardDeck = cardDeck;
            this.logger = logger;
        }

        protected override void ExecuteCmdlet()
        {
            var card = cardDeck.Draw();

            logger.LogInformation("Card Drawn: {Name}", card.ToString());
        }
    }
}
