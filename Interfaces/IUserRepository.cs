using Shopping_Cart.Models;

namespace Shopping_Cart.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string username);
        Task<User> RegisterAsync(User user);
    }
}
