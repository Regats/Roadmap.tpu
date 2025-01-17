using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> DeleteUserAsync(Guid userID)
        {
            try
            {
               await _userRepository.DeleteUserAsync(userID);
               return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении пользлвателя");
                return false;
            }
        }

        public async Task<UserDTO> GetUserByGuidAsync(Guid userID)
        {
            var user = await _userRepository.GetUserByGuidAsync(userID);
            return user;
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return users;
        }

        public async Task<bool> UpdateUserAsync(Guid userID)
        {
            try
            {
                await _userRepository.UpdateUserAsync(userID);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении пользователя");
                return false;
            }
        }
    }
}
