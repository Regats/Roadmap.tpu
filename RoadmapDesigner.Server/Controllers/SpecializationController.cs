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
    [Route("api/specializations")] // Маршрут для контроллера специализаций
    public class SpecializationController : ControllerBase
    {
        private readonly ILogger<SpecializationController> _logger; // Логгер для записи информации и ошибок
        private readonly ISpecializationService _specializationService;  // Сервис для работы со специализациями

        // Конструктор для внедрения зависимостей
        public SpecializationController(ILogger<SpecializationController> logger, ISpecializationService specializationService)
        {
            _logger = logger; // Инициализация логгера
            _specializationService = specializationService; // Инициализация сервиса
        }

        // Обработчик для GET запроса на получение списка специализаций по идентификатору направления подготовки
        [HttpGet("list/{dirTrainingUuid}")]
        public async Task<ActionResult<List<SpecializationDTO>>> GetListSpecializationsByDirTrainingUuid(Guid dirTrainingUuid)
        {
            try
            {
                _logger.LogInformation($"Запрос на получение списка специализаций для направления подготовки с UUID: {dirTrainingUuid}");

                // Проверка на валидность UUID
                if (dirTrainingUuid == Guid.Empty)
                {
                    _logger.LogWarning("Передан невалидный UUID направления подготовки.");
                    return BadRequest("Невалидный UUID направления подготовки."); // Возвращаем 400, если UUID недействителен
                }

                // Вызов метода сервиса для получения списка специализаций
                var listSpecializations = await _specializationService.GetListSpecializationsByDirTrainingUuid(dirTrainingUuid);

                // Проверка на null
                if (listSpecializations == null)
                {
                    _logger.LogWarning($"Специализации для направления подготовки с UUID: {dirTrainingUuid} не найдены.");
                    return NotFound($"Специализации для направления подготовки с UUID: {dirTrainingUuid} не найдены."); // Возвращаем 404, если не найдено
                }

                _logger.LogInformation($"Успешно получено {listSpecializations.Count} специализаций для направления подготовки с UUID: {dirTrainingUuid}");
                return Ok(listSpecializations); // Возвращаем 200 OK с данными
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении списка специализаций для направления подготовки с UUID: {dirTrainingUuid}");
                return StatusCode(500, "Произошла внутренняя ошибка сервера."); // Возвращаем 500 в случае ошибки
            }

        }

        // Обработчик для GET запроса на получение специализации по UUID
        [HttpGet("{specUuid}")]
        public async Task<ActionResult<SpecializationDTO>> GetSpecializationByUuid(Guid specUuid)
        {
            try
            {
                _logger.LogInformation($"Запрос на получение специализации с UUID: {specUuid}");

                // Проверка на валидность UUID
                if (specUuid == Guid.Empty)
                {
                    _logger.LogWarning("Передан невалидный UUID специализации.");
                    return BadRequest("Невалидный UUID специализации.");  // Возвращаем 400, если UUID недействителен
                }

                // Вызов метода сервиса для получения специализации
                var specialization = await _specializationService.GetSpecializationByUuid(specUuid);

                // Проверка на null
                if (specialization == null)
                {
                    _logger.LogWarning($"Специализация с UUID: {specUuid} не найдена.");
                    return NotFound($"Специализация с UUID: {specUuid} не найдена."); // Возвращаем 404, если не найдено
                }

                _logger.LogInformation($"Успешно получена специализация с UUID: {specUuid}");
                return Ok(specialization); // Возвращаем 200 OK с данными
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении специализации с UUID: {specUuid}");
                return StatusCode(500, "Произошла внутренняя ошибка сервера."); // Возвращаем 500 в случае ошибки
            }

        }
    }
}