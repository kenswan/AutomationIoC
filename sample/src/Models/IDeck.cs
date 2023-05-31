// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Sample.Models;

public interface IDeck
{
    Card Draw();

    void Load();
}
