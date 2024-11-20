namespace TodoApi.Models;

public class Comment {
  public long Id { get; set; }
  public string? Text { get; set; }

  public long TodoItemId { get; set; }  // Foreign key

  public TodoItem? TodoItem { get; set; }

}