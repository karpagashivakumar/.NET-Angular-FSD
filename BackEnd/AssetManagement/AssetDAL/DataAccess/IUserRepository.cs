using AssetDAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetDAL.DataAccess
{
    public interface IUserRepository
    {
        
        // Authenticates a user by username and password.
        Task<User?> AuthenticateAsync(string username, string password);

        // Retrieves a user by their ID.
        Task<User?> GetByIdAsync(long userId);

        // Retrieves all users.
        Task<IEnumerable<User>> GetAllAsync();

        // Creates a new user.
        Task<User> CreateAsync(User user);

        // Updates an existing user's information.
        Task<User> UpdateAsync(User user);

        // Deactivates a user (sets IsActive to false).
        Task<bool> DeactivateAsync(long userId);

        // Reactivates a user (sets IsActive to true).
        Task<bool> ActivateAsync(long userId);

        // Checks if a username already exists.
        Task<bool> UsernameExistsAsync(string username);

        // Checks if an email already exists.
        Task<bool> EmailExistsAsync(string email);

        Task<User> GetByEmailAsync(string email);
        Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword);


    }
}

