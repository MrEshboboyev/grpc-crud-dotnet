using GrpcCrudExampleDotnet.Models;

namespace GrpcCrudExampleDotnet.Services;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<List<User>> GetAllUsersAsync();
}
