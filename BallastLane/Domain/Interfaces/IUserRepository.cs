using BallastLane.Domain.Entities;

namespace BallastLane.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User> GetByUsernameAndPasswordAsync(string username, string password);
        Task<User> GetByIdAsync(int id);
    }
}
