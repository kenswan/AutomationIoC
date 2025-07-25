// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using EFCorePowerShellSample.Models;

namespace EFCorePowerShellSample.Context;

public interface IToDoStorageAdapter
{
    IQueryable<ToDoEntity> SelectAllToDos();

    Task<ToDoEntity> SelectToDoByIdAsync(Guid id);

    Task<ToDoEntity> InsertToDoAsync(ToDoEntity toDoEntity);

    Task<ToDoEntity> UpdateToDoAsync(ToDoEntity toDoEntity);

    Task<bool> DeleteToDoAsync(Guid id);
}
