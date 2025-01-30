using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Interfaces
{
    public interface IUserRepository
    {
        // Метод для асинхронного добавления пользователя
        Task AddUserAsync(User user);

        // Метод для асинхронного удаления пользователя по UUID
        Task DeleteUserAsync(Guid userUuid);

        // Метод для асинхронного обновления пользователя по UUID
        Task UpdateUserAsync(Guid userUuid);

        // Метод для асинхронного получения пользователя по UUID
        Task<UserDTO> GetUserByGuidAsync(Guid userUuid);

        Task<UserDTO> GetUserByLoginAsync(string login);

        // Метод для асинхронного получения списка всех пользователей
        Task<List<UserDTO>> GetUsersAsync();
    }
}