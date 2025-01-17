using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByGuidAsync(Guid userID);
        Task<List<UserDTO>> GetUsersAsync();
        Task<bool> UpdateUserAsync(Guid userID);
        Task<bool> DeleteUserAsync(Guid userID);
    }
}
