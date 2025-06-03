using GrpcCrudExampleDotnet.Models;
using GrpcCrudExampleDotnet.Services;

namespace GrpcCrudExampleDotnet.Repository;

public class UserRepository : IUserRepository
{
    private static List<User> _users = [];
    private static int _nextId = 1;

    public Task<User> CreateUserAsync(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User?> UpdateUserAsync(User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Age = user.Age;
            existingUser.Phone = user.Phone;
            return Task.FromResult(existingUser);
        }
        return Task.FromResult<User?>(null);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return Task.FromResult(_users.ToList());
    }
}