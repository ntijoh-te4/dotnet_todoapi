
using TodoApi.Models;

namespace TodoApi.Dtos;
public class TodoItemDto {


  public TodoItemDto(TodoItem todoitem) {
    Id = todoitem.Id;
    Name = todoitem.Name;
    IsComplete = todoitem.IsComplete;

    if (todoitem.Comments == null) {
      Comments = [];
    } else {
      Comments = todoitem.Comments
          .Select(comment => new CommentDto(comment))
          .ToList();
    }
  }

  public long Id { get; set; }
  public string Name { get; set; }
  public bool IsComplete { get; set; }
  public List<CommentDto> Comments { get; set; } = [];
}