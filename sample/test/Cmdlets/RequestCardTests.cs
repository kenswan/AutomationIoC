// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Tools;
using AutomationIoC.Sample.Cmdlets;
using AutomationIoC.Sample.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AutomationIoC.Sample.Test.Cmdlets;

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
