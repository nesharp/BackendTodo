using TodoBackend.Models;

namespace TodoBackend.Interfaces;

public interface IUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public List<Todo> Todos { get; set; }
}
public interface IUserLoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public interface IUserRegisterDto : IUserLoginDto
{
    public string Name { get; set; }
}
public enum UserRole
{
    Admin, 
    User,
    Owner
}