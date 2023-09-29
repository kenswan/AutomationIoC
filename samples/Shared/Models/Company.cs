// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationSamples.Shared.Models;

public class Company : RecordEntity
{
    public string CompanyName { get; set; }

    public string Description { get; set; }

    public string Slogan { get; set; }
}
