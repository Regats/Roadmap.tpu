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
    [Route("api/disciplines")] // Маршрут для контроллера дисциплин
    public class DisciplinesController : ControllerBase
    {
        private readonly ILogger<DisciplinesController> _logger; // Логгер для записи информации и ошибок
        private readonly IDisciplineService _disciplineService; // Сервис для работы с дисциплинами

        // Конструктор для внедрения зависимостей
        public DisciplinesController(ILogger<DisciplinesController> logger, IDisciplineService disciplineService)
        {
            _logger = logger;   // Инициализация логгера
            _disciplineService = disciplineService; // Инициализация сервиса
        }

        // Обработчик для GET запроса на получение списка дисциплин
        [HttpGet]
        public async Task<ActionResult<List<DisciplineDTO>>> GetListDisciplines()
        {
            try
            {
                _logger.LogInformation("Запрос на получение списка дисциплин.");

                // Вызов метода сервиса для получения списка дисциплин
                var listDisciplines = await _disciplineService.GetListDisciplinesAsync();

                // Проверка на null
                if (listDisciplines == null)
                {
                    _logger.LogWarning("Список дисциплин пуст.");
                    return NotFound("Список дисциплин пуст."); // Возвращаем 404, если список пуст
                }

                _logger.LogInformation($"Успешно получено {listDisciplines.Count} дисциплин.");
                return Ok(listDisciplines); // Возвращаем 200 OK с данными
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении списка дисциплин.");
                return StatusCode(500, "Произошла внутренняя ошибка сервера."); // Возвращаем 500 в случае ошибки
            }
        }
    }
}