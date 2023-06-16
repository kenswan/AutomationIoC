// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace ConsolesSample.Models;

public class Product : RecordEntity
{
    public string ProductName { get; set; }

    public decimal Price { get; set; }
}
