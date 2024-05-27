using BallastLane.Application.Interfaces;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Interfaces;

namespace BallastLane.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // In a real-world scenario, ensure passwords are hashed and check for existing users
            return await _userRepository.AddAsync(user);
        }

        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            // In a real-world scenario, check hashed passwords
            return await _userRepository.GetByUsernameAndPasswordAsync(username, password);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
    }
}
