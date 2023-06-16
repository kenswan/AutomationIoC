// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Tools;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PSCmdletsSample;
using PSCmdletsSample.Cmdlets;
using PSCmdletsSample.Models;
using Xunit;

namespace PSCmdletToolsSample.Cmdlets;

public class RequestCardTests
{
    [Fact]
    public void ShouldRequestCard()
    {
        var deckMock = new Mock<IDeck>();
        var expectedCard = new Card { Rank = Rank.Ace, Suit = Suit.Spade };

        using IAutomationCommand<RequestCard> requestCardCommand =
            AutomationSandbox.CreateContext<RequestCard, Startup>(services =>
                services.AddTransient(_ => deckMock.Object));

        deckMock.Setup(deck => deck.Draw()).Returns(expectedCard);

        Card actualCard = requestCardCommand.RunCommand<Card>().First();

        actualCard.Should().BeEquivalentTo(expectedCard);
    }
}
