using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class TodoContext : DbContext {
  public TodoContext(DbContextOptions<TodoContext> options)
      : base(options) {
  }

  public DbSet<TodoItem> TodoItems { get; set; } = null!;
  public DbSet<Comment> Comments { get; set; } = null!;

  //In TodoContext.cs
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Comment>()
                .HasOne(c => c.TodoItem)                 // Each Comment has one associated TodoItem.
                .WithMany(t => t.Comments)               // Each TodoItem can have many Comments.
                .HasForeignKey(c => c.TodoItemId);       // The foreign key is Comment.TodoItemId.
  }
}