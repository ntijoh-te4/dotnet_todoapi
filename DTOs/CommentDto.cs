using TodoApi.Models;

namespace TodoApi.Dtos;

public class CommentDto {
  public CommentDto(Comment comment) {
    Id = comment.Id;
    Text = comment.Text ?? string.Empty; // Use the null coalescing operator
  }

  public long Id { get; set; }
  public string Text { get; set; }
}