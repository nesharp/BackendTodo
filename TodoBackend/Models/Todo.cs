using System.ComponentModel.DataAnnotations;
using TodoBackend.Interfaces;

namespace TodoBackend.Models;
public class TodoDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = "";
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public bool? IsCompleted { get; set; }

    public DateTime AssignedTo { get; set; } = DateTime.Now;
}
public class Todo: ITodo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime AssignedTo { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

public class TodoReturn : TodoDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int UserId { get; set; }
}
public class TodoUpdateDto 
{
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string? Title { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public DateTime AssignedTo { get; set; } = DateTime.Now;
}
public class ChangeStateDto
{
    public bool IsCompleted { get; set; }
}