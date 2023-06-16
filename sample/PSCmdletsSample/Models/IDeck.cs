// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace PSCmdletsSample.Models;

public interface IDeck
{
    Card Draw();

    void Load();
}
