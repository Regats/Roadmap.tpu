using Microsoft.AspNetCore.Http;
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
    [Route("api/direction-trainings")] // Маршрут для контроллера направлений обучения
    public class DirectionTrainingsController : ControllerBase
    {
        private readonly IDirectionTrainingService _directionTrainingService; // Сервис для работы с направлениями обучения
        private readonly ILogger<DirectionTrainingsController> _logger; // Логгер для записи информации и ошибок

        // Конструктор для внедрения зависимостей
        public DirectionTrainingsController(IDirectionTrainingService directionTrainingService, ILogger<DirectionTrainingsController> logger)
        {
            _logger = logger; // Инициализация логгера
            _directionTrainingService = directionTrainingService; // Инициализация сервиса
        }

        // Обработчик для GET запроса на получение всех областей обучения
        [HttpGet]
        public async Task<ActionResult<List<TrainingArea>>> GetAllTrainingAreas()
        {
            try
            {
                _logger.LogInformation("Запрос на получение всех областей обучения.");

                // Вызов метода сервиса для получения данных
                var trainingAreas = await _directionTrainingService.GetAllTrainingAreas();

                // Проверка на null
                if (trainingAreas == null)
                {
                    _logger.LogWarning("Области обучения не найдены.");
                    return NotFound("Области обучения не найдены."); // Возвращаем 404, если ничего не найдено
                }

                _logger.LogInformation($"[{DateTime.Now:G}] Успешно получено {trainingAreas.Count} областей обучения.");
                return Ok(trainingAreas); // Возвращаем 200 OK с данными
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении областей обучения.");
                return StatusCode(500, "Произошла внутренняя ошибка сервера."); // Возвращаем 500 в случае ошибки
            }
        }

        // Обработчик для GET запроса на получение деталей версии направления обучения по UUID
        [HttpGet("direction-training-details/{directionTrainingUuid}")]
        public async Task<ActionResult<VersionsDirectionTrainingDTO>> GetVersionDirectionTrainingDetails(Guid directionTrainingUuid)
        {
            try
            {
                _logger.LogInformation($"Запрос на получение деталей версии направления обучения с UUID: {directionTrainingUuid}");

                // Проверка на валидность UUID
                if (directionTrainingUuid == Guid.Empty)
                {
                    _logger.LogWarning("Передан невалидный UUID.");
                    return BadRequest("Невалидный UUID.");  // Возвращаем 400, если UUID недействителен
                }

                // Вызов метода сервиса для получения деталей направления обучения
                var detailsDirectionTraining = await _directionTrainingService.GetVersionDirectionTrainingDetails(directionTrainingUuid);

                // Проверка на null
                if (detailsDirectionTraining == null)
                {
                    _logger.LogWarning($"Версия направления обучения с UUID: {directionTrainingUuid} не найдена.");
                    return NotFound($"Версия направления обучения с UUID: {directionTrainingUuid} не найдена."); // Возвращаем 404, если не найдено
                }

                _logger.LogInformation($"Успешно получены детали версии направления обучения с UUID: {directionTrainingUuid}");
                return Ok(detailsDirectionTraining); // Возвращаем 200 OK с данными
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении деталей версии направления обучения с UUID: {directionTrainingUuid}");
                return StatusCode(500, "Произошла внутренняя ошибка сервера.");  // Возвращаем 500 в случае ошибки
            }
        }
    }
}