using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await SeedData(app);

app.Run();

async Task SeedData(WebApplication app) {

  using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
    await context.Database.EnsureCreatedAsync();

    if (!context.TodoItems.Any()) {

      var todoItem = new TodoItem {
        Name = "Test item from seeder",
        IsComplete = false
      };

      var comment = new Comment {
        Text = "Test comment from seeder",
        TodoItem = todoItem
      };

      todoItem.Comments.Add(comment);

      await context.TodoItems.AddAsync(todoItem);


      todoItem = new TodoItem {
        Name = "Test item from seeder 2",
        IsComplete = false
      };

      comment = new Comment {
        Text = "Test comment from seeder 2",
        TodoItem = todoItem
      };

      todoItem.Comments.Add(comment);

      await context.TodoItems.AddAsync(todoItem);


      await context.SaveChangesAsync();
    }
  }
}
