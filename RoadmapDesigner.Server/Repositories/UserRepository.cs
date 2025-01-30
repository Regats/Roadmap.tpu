using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RoadmapContext _context;  // Контекст базы данных
        private readonly ILogger<UserRepository> _logger; // Логгер для записи информации и ошибок

        // Конструктор для внедрения зависимостей
        public UserRepository(RoadmapContext context, ILogger<UserRepository> logger)
        {
            _context = context;   // Инициализация контекста
            _logger = logger;  // Инициализация логгера
        }

        // Метод для асинхронного добавления пользователя
        public async Task AddUserAsync(User user)
        {
            try
            {
                _logger.LogInformation($"Начало добавления пользователя с UUID: {user.UserId}");

                _context.Users.Add(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                _logger.LogInformation($"Успешно добавлен пользователь с UUID: {user.UserId}");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных.");
                throw;  // Пробрасываем исключение на уровень сервиса
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неожиданная ошибка при добавлении пользователя.");
                throw; // Пробрасываем исключение на уровень сервиса
            }

        }

        // Метод для асинхронного удаления пользователя по UUID
        public async Task DeleteUserAsync(Guid userUuid)
        {
            try
            {
                _logger.LogInformation($"Начало удаления пользователя с UUID: {userUuid}");

                // Получаем пользователя из контекста
                var user = await _context.Users.FindAsync(userUuid).ConfigureAwait(false);
                // Удаляем пользователя
                _context.Users.Remove(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                _logger.LogInformation($"Успешно удален пользователь с UUID: {userUuid}");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных.");
                throw; // Пробрасываем исключение на уровень сервиса
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при удалении пользователя с UUID: {userUuid}");
                throw; // Пробрасываем исключение на уровень сервиса
            }

        }

        public async Task<UserDTO> GetUserByLoginAsync(string login)
        {
            try
            {
                _logger.LogInformation($"Начало запроса на получение пользователя с login: {login}");

                // Получаем пользователя по UUID
                var user = await _context.Users
                    .Where(u => u.Login == login)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                // Проверка на null
                if (user == null)
                {
                    _logger.LogWarning($"Пользователь с login: {login} не найден.");
                    return null;
                }

                // Получаем роль пользователя
                var role = await _context.UserRoles.FindAsync(user.RoleId).ConfigureAwait(false);

                _logger.LogInformation($"Успешно получен пользователь с login: {login}");
                // Преобразуем пользователя в DTO
                return new UserDTO
                {
                    UserId = user.UserId,    // Копируем идентификатор
                    LastName = user.LastName, // Копируем фамилию
                    FirstName = user.FirstName,  // Копируем имя
                    MiddleName = user.MiddleName, // Копируем отчество
                    Email = user.Email,  // Копируем email
                    Login = user.Login, // Копируем логин
                    Role = role?.Role // Копируем роль
                };
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных.");
                throw;   // Пробрасываем исключение на уровень сервиса
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении пользователя с login: {login}");
                throw; // Пробрасываем исключение на уровень сервиса
            }

        }

        // Метод для асинхронного получения пользователя по UUID
        public async Task<UserDTO> GetUserByGuidAsync(Guid userUuid)
        {
            try
            {
                _logger.LogInformation($"Начало запроса на получение пользователя с UUID: {userUuid}");

                // Получаем пользователя по UUID
                var user = await _context.Users.FindAsync(userUuid).ConfigureAwait(false);

                // Проверка на null
                if (user == null)
                {
                    _logger.LogWarning($"Пользователь с UUID: {userUuid} не найден.");
                    return null;
                }

                // Получаем роль пользователя
                var role = await _context.UserRoles.FindAsync(user.RoleId).ConfigureAwait(false);

                _logger.LogInformation($"Успешно получен пользователь с UUID: {userUuid}");
                // Преобразуем пользователя в DTO
                return new UserDTO
                {
                    UserId = user.UserId,    // Копируем идентификатор
                    LastName = user.LastName, // Копируем фамилию
                    FirstName = user.FirstName,  // Копируем имя
                    MiddleName = user.MiddleName, // Копируем отчество
                    Email = user.Email,  // Копируем email
                    Login = user.Login, // Копируем логин
                    Role = role?.Role // Копируем роль
                };
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных.");
                throw;   // Пробрасываем исключение на уровень сервиса
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении пользователя с UUID: {userUuid}");
                throw; // Пробрасываем исключение на уровень сервиса
            }

        }

        // Метод для асинхронного получения списка всех пользователей
        public async Task<List<UserDTO>> GetUsersAsync()
        {
            try
            {
                _logger.LogInformation("Начало запроса на получение списка всех пользователей.");
                // Получаем всех пользователей с их ролями
                var users = await _context.Users
                    .Include(u => u.Role)  // Загружаем роли
                     .ToListAsync()
                      .ConfigureAwait(false);

                _logger.LogInformation($"Запрос на получение списка всех пользователей вернул {users.Count} записей.");

                // Преобразуем пользователей в DTO
                var userDtos = users.Select(u => new UserDTO
                {
                    UserId = u.UserId,    // Копируем идентификатор
                    LastName = u.LastName, // Копируем фамилию
                    FirstName = u.FirstName,   // Копируем имя
                    MiddleName = u.MiddleName,  // Копируем отчество
                    Login = u.Login,   // Копируем логин
                    Email = u.Email,  // Копируем email
                    Role = u.Role.Role // Копируем роль
                }).ToList();

                return userDtos;  // Возвращаем список DTO
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных.");
                throw;  // Пробрасываем исключение на уровень сервиса
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении списка всех пользователей.");
                return new List<UserDTO>();  // Возвращаем пустой список в случае ошибки
            }

        }


        // Метод для асинхронного обновления пользователя по UUID
        public async Task UpdateUserAsync(Guid userUuid)
        {
            try
            {
                _logger.LogInformation($"Начало обновления пользователя с UUID: {userUuid}");
                // Находим пользователя для обновления
                var user = await _context.Users.FindAsync(userUuid).ConfigureAwait(false);

                _context.Users.Update(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                _logger.LogInformation($"Успешно обновлен пользователь с UUID: {userUuid}");

            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных.");
                throw;  // Пробрасываем исключение на уровень сервиса
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при обновлении пользователя с UUID: {userUuid}");
                throw;  // Пробрасываем исключение на уровень сервиса
            }
        }
    }
}