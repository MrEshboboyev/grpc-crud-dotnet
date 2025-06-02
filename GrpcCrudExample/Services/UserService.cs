using Grpc.Core;
using GrpcCrudExample;

namespace GrpcCrudExampleDotnet.Services;

public class UserService : GrpcCrudExample.UserService.UserServiceBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public override async Task<UserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Creating user with name: {Name}", request.Name);

            var user = new Models.User
            {
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                Phone = request.Phone
            };

            var createdUser = await _userRepository.CreateUserAsync(user);

            return new UserResponse
            {
                Success = true,
                Message = "User created successfully",
                User = new User
                {
                    Id = createdUser.Id,
                    Name = createdUser.Name,
                    Email = createdUser.Email,
                    Age = createdUser.Age,
                    Phone = createdUser.Phone
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return new UserResponse
            {
                Success = false,
                Message = $"Error creating user: {ex.Message}"
            };
        }
    }

    public override async Task<UserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting user with ID: {Id}", request.Id);

            var user = await _userRepository.GetUserByIdAsync(request.Id);

            if (user == null)
            {
                return new UserResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            return new UserResponse
            {
                Success = true,
                Message = "User retrieved successfully",
                User = new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Age = user.Age,
                    Phone = user.Phone
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user");
            return new UserResponse
            {
                Success = false,
                Message = $"Error getting user: {ex.Message}"
            };
        }
    }

    public override async Task<UserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {Id}", request.Id);

            var user = new Models.User
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                Phone = request.Phone
            };

            var updatedUser = await _userRepository.UpdateUserAsync(user);

            if (updatedUser == null)
            {
                return new UserResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            return new UserResponse
            {
                Success = true,
                Message = "User updated successfully",
                User = new User
                {
                    Id = updatedUser.Id,
                    Name = updatedUser.Name,
                    Email = updatedUser.Email,
                    Age = updatedUser.Age,
                    Phone = updatedUser.Phone
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user");
            return new UserResponse
            {
                Success = false,
                Message = $"Error updating user: {ex.Message}"
            };
        }
    }

    public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID: {Id}", request.Id);

            var deleted = await _userRepository.DeleteUserAsync(request.Id);

            return new DeleteUserResponse
            {
                Success = deleted,
                Message = deleted ? "User deleted successfully" : "User not found"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user");
            return new DeleteUserResponse
            {
                Success = false,
                Message = $"Error deleting user: {ex.Message}"
            };
        }
    }

    public override async Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("Getting all users");

            var users = await _userRepository.GetAllUsersAsync();

            var response = new GetAllUsersResponse
            {
                Success = true,
                Message = "Users retrieved successfully"
            };

            foreach (var user in users)
            {
                response.Users.Add(new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Age = user.Age,
                    Phone = user.Phone
                });
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return new GetAllUsersResponse
            {
                Success = false,
                Message = $"Error getting users: {ex.Message}"
            };
        }
    }
}