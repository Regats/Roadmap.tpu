using RoadmapDesigner.Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IUserService
    {
        // Метод для асинхронного получения пользователя по UUID
        Task<UserDTO> GetUserByGuidAsync(Guid userUuid);

        // Метод для асинхронного получения списка всех пользователей
        Task<List<UserDTO>> GetUsersAsync();

        // Метод для асинхронного обновления пользователя по UUID
        Task<bool> UpdateUserAsync(Guid userUuid);

        // Метод для асинхронного удаления пользователя по UUID
        Task<bool> DeleteUserAsync(Guid userUuid);
    }
}