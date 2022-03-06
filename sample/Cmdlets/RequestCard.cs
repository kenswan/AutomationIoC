using Microsoft.Extensions.Logging;
using AutomationIoC.Sample.Models;
using System.Management.Automation;

namespace AutomationIoC.Sample.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Request, "Card")]
    public class RequestCard : IoCShell
    {
        [AutoInject]
        public ILogger<RequestCard> logger { get; set; }

        [AutoInject]
        public Deck cardDeck { get; set; }

        protected override void ExecuteCmdlet()
        {
            var card = cardDeck.Draw();

            logger.LogInformation("Card Drawn: {Name}", card.ToString());
        }
    }
}
