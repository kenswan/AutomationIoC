// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationSamples.Shared.Models;

public class ReportOptions
{
    public int? MaxResults { get; set; }

    public string ReportType { get; set; } = nameof(ReportCategory.Product);

    public string Disclaimer { get; set; }
}
