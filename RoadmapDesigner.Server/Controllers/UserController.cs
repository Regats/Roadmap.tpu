using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("api/users")] // Маршрут для контроллера пользователей
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger; // Логгер для записи информации и ошибок
        private readonly IUserService _userService;  // Сервис для работы с пользователями

        // Конструктор для внедрения зависимостей
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _logger = logger; // Инициализация логгера
            _userService = userService; // Инициализация сервиса
        }

        // Обработчик для GET запроса на получение пользователя по UUID
        [HttpGet("{userUuid}")]
        public async Task<ActionResult<UserDTO>> GetUserByUuid(Guid userUuid)
        {
            try
            {
                _logger.LogInformation($"Запрос на получение пользователя с UUID: {userUuid}");

                // Проверка на валидность UUID
                if (userUuid == Guid.Empty)
                {
                    _logger.LogWarning("Передан невалидный UUID пользователя.");
                    return BadRequest("Невалидный UUID пользователя."); // Возвращаем 400, если UUID недействителен
                }
                // Вызов метода сервиса для получения пользователя
                var user = await _userService.GetUserByGuidAsync(userUuid);

                // Проверка на null
                if (user == null)
                {
                    _logger.LogWarning($"Пользователь с UUID: {userUuid} не найден.");
                    return NotFound($"Пользователь с UUID: {userUuid} не найден."); // Возвращаем 404, если не найдено
                }
                _logger.LogInformation($"Успешно получен пользователь с UUID: {userUuid}");

                return Ok(user); // Возвращаем 200 OK с данными
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении пользователя с UUID: {userUuid}");
                return StatusCode(500, "Произошла внутренняя ошибка сервера."); // Возвращаем 500 в случае ошибки
            }
        }

        // Обработчик для GET запроса на получение всех пользователей
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Запрос на получение списка всех пользователей.");
                // Вызов метода сервиса для получения списка пользователей
                var users = await _userService.GetUsersAsync();
                // Проверка на null
                if (users == null)
                {
                    _logger.LogWarning("Список пользователей пуст.");
                    return NotFound("Список пользователей пуст.");// Возвращаем 404, если список пуст
                }
                _logger.LogInformation($"Успешно получено {users.Count} пользователей.");

                return Ok(users); // Возвращаем 200 OK с данными
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении списка пользователей.");
                return StatusCode(500, "Произошла внутренняя ошибка сервера."); // Возвращаем 500 в случае ошибки
            }
        }
    }
}