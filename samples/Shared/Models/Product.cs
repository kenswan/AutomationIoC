// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationSamples.Shared.Models;

public class Product : RecordEntity
{
    public string ProductName { get; set; }

    public decimal Price { get; set; }
}
