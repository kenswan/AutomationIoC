﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutomationIoC.Sample.Models;
using System.Management.Automation;

namespace AutomationIoC.Sample.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Request, "Card")]
    public class RequestCard : IoCShell
    {
        protected override void ExecuteCmdlet(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<RequestCard>>();
            var card = scope.ServiceProvider.GetRequiredService<Deck>().Draw();

            logger.LogInformation("Card Drawn: {Name}", card.ToString());
        }
    }
}
