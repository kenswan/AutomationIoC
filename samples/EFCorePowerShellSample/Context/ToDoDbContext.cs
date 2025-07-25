// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using EFCorePowerShellSample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCorePowerShellSample.Context;

public class ToDoDbContext(DbContextOptions<ToDoDbContext> options) : DbContext(options), IToDoStorageAdapter
{
    public DbSet<ToDoEntity> ToDos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ToDoEntity>()
            .HasKey(toDo => toDo.Id);
    }

    public IQueryable<ToDoEntity> SelectAllToDos() => this.ToDos;

    public async Task<ToDoEntity> SelectToDoByIdAsync(Guid id) =>
        await this.ToDos.FirstOrDefaultAsync(toDo => toDo.Id == id);

    public async Task<ToDoEntity> InsertToDoAsync(ToDoEntity toDoEntity)
    {
        EntityEntry<ToDoEntity> newToDoEntity =
            await this.ToDos.AddAsync(toDoEntity);

        await SaveChangesAsync();

        return newToDoEntity.Entity;
    }

    public async Task<ToDoEntity> UpdateToDoAsync(ToDoEntity toDoEntity)
    {
        EntityEntry<ToDoEntity> updatedToDoEntity =
            this.ToDos.Update(toDoEntity);

        await SaveChangesAsync();

        return updatedToDoEntity.Entity;
    }

    public async Task<bool> DeleteToDoAsync(Guid id)
    {
        ToDoEntity toDoEntity = await this.ToDos.FindAsync(id);

        if (toDoEntity is null)
        {
            return false;
        }

        this.ToDos.Remove(toDoEntity);
        await SaveChangesAsync();

        return true;
    }
}
