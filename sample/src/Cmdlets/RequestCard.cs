using AutomationIoC.Sample.Models;
using Microsoft.Extensions.Logging;
using System.Management.Automation;

namespace AutomationIoC.Sample.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Request, "Card")]
    public class RequestCard : IoCShell<Startup>
    {
        [AutomationDependency]
        protected IDeck CardDeck { get; set; }

        [AutomationDependency]
        private readonly ILogger<RequestCard> logger = default;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var card = CardDeck.Draw();

            logger.LogInformation("Card Drawn: {Name}", card.ToString());

            WriteObject(card);
        }
    }
}
