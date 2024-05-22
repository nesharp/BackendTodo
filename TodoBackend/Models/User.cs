using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TodoBackend.Interfaces;

namespace TodoBackend.Models;

public class User : IUser
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; }
    
    [JsonIgnore]
    public string PasswordHash { get; set; }
    public List<Todo> Todos { get; set; }
    
    [EnumDataType(typeof(UserRole))]
    public UserRole Role { get; set; }
    
}

public class UserLoginDto: IUserLoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
//i dont know if i right
public class UserDto : UserLoginDto, IUserRegisterDto
{
    public string Name { get; set; }
}
