using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task DeleteUserAsync(Guid userID);
        Task UpdateUserAsync(Guid userID);
        Task<UserDTO> GetUserByGuidAsync(Guid userID);
        Task<List<UserDTO>> GetUsersAsync();
    }
}
