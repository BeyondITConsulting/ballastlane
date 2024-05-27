using BallastLane.Domain.Entities;

namespace BallastLane.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> AuthenticateUserAsync(string username, string password);
        Task<User> GetUserByIdAsync(int id);
    }
}
