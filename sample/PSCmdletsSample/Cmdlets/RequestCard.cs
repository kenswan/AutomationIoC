// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets;
using Microsoft.Extensions.Logging;
using PSCmdletsSample.Models;
using System.Management.Automation;

namespace PSCmdletsSample.Cmdlets;

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

        Card card = CardDeck.Draw();

        logger.LogInformation("Card Drawn: {Name}", card.ToString());

        WriteObject(card);
    }
}
