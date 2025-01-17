using Microsoft.Extensions.Logging;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;  // Репозиторий для работы с пользователями
        private readonly ILogger<UserService> _logger;   // Логгер для записи информации и ошибок

        // Конструктор для внедрения зависимостей
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository; // Инициализация репозитория
            _logger = logger; // Инициализация логгера
        }

        // Метод для асинхронного удаления пользователя по UUID
        public async Task<bool> DeleteUserAsync(Guid userUuid)
        {
            try
            {
                _logger.LogInformation($"Начало процесса удаления пользователя с UUID: {userUuid}");
                await _userRepository.DeleteUserAsync(userUuid);

                _logger.LogInformation($"Успешно удален пользователь с UUID: {userUuid}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при удалении пользователя с UUID: {userUuid}");
                return false; // Возвращаем false в случае ошибки
            }

        }

        // Метод для асинхронного получения пользователя по UUID
        public async Task<UserDTO> GetUserByGuidAsync(Guid userUuid)
        {
            try
            {
                _logger.LogInformation($"Начало процесса получения пользователя с UUID: {userUuid}");
                var user = await _userRepository.GetUserByGuidAsync(userUuid);

                _logger.LogInformation($"Успешно получен пользователь с UUID: {userUuid}");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении пользователя с UUID: {userUuid}");
                return null; // Возвращаем null в случае ошибки
            }
        }

        // Метод для асинхронного получения списка всех пользователей
        public async Task<List<UserDTO>> GetUsersAsync()
        {
            try
            {
                _logger.LogInformation("Начало процесса получения списка всех пользователей.");
                var users = await _userRepository.GetUsersAsync();

                _logger.LogInformation("Успешно получен список всех пользователей.");
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении списка всех пользователей.");
                return null;   // Возвращаем null в случае ошибки
            }
        }

        // Метод для асинхронного обновления пользователя по UUID
        public async Task<bool> UpdateUserAsync(Guid userUuid)
        {
            try
            {
                _logger.LogInformation($"Начало процесса обновления пользователя с UUID: {userUuid}");
                await _userRepository.UpdateUserAsync(userUuid);

                _logger.LogInformation($"Успешно обновлен пользователь с UUID: {userUuid}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при обновлении пользователя с UUID: {userUuid}");
                return false;  // Возвращаем false в случае ошибки
            }
        }
    }
}