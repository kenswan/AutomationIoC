// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationSamples.Shared.Models;

public class Database : RecordEntity
{
    public string Name { get; set; }

    public string Collation { get; set; }

    public string Engine { get; set; }

    public IList<string> Columns { get; set; }
}
