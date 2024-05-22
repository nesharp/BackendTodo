using TodoBackend.Models;

namespace TodoBackend.Interfaces;

public interface ITodo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime AssignedTo { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}