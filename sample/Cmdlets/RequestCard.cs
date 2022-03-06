using AutomationIoC.Sample.Models;
using Microsoft.Extensions.Logging;
using System.Management.Automation;

namespace AutomationIoC.Sample.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Request, "Card")]
    public class RequestCard : IoCShell
    {
        [AutomationDependency]
        public ILogger<RequestCard> logger { get; set; }

        [AutomationDependency]
        public Deck cardDeck { get; set; }

        protected override void ExecuteCmdlet()
        {
            var card = cardDeck.Draw();

            logger.LogInformation("Card Drawn: {Name}", card.ToString());
        }
    }
}
