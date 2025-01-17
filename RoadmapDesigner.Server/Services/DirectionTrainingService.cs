using Microsoft.Extensions.Logging;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Services
{
    public class DirectionTrainingService : IDirectionTrainingService
    {

        private readonly IDirectionTrainingRepository _versionsDirectionTrainingRepository;
        private readonly ILogger<DirectionTrainingService> _logger;

        // Конструктор, принимающий репозиторий и логгер
        public DirectionTrainingService(ILogger<DirectionTrainingService> logger, IDirectionTrainingRepository directionTrainingRepository)
        {
            _logger = logger;   // Инициализация логгера
            _versionsDirectionTrainingRepository = directionTrainingRepository; // Инициализация репозитория
        }


        // Метод для получения деталей версии направления обучения
        public async Task<VersionsDirectionTrainingDTO> GetVersionDirectionTrainingDetails(Guid uuid)
        {
            try
            {
                _logger.LogInformation($"Начало получения детали версии направления обучения с UUID: {uuid}.");

                // Вызов метода репозитория
                var versionDirectionTraining = await _versionsDirectionTrainingRepository.GetDetailsAsync(uuid);

                _logger.LogInformation($"Успешно получена детали версии направления обучения с UUID: {uuid}.");
                return versionDirectionTraining;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении детали версии направления обучения с UUID: {uuid}.");
                return null; // Возвращаем null, если произошла ошибка или версия не найдена
            }
        }

        // Метод для получения всех областей обучения с направлениями обучения
        public async Task<List<TrainingArea>> GetAllTrainingAreas()
        {
            try
            {
                _logger.LogInformation("Начало процесса получения и сортировки всех направлений обучения.");
                // Получаем список областей обучения
                var listTrainingAreas = TrainingAreas.GetTrainingAreas();

                // Получаем список направлений обучения
                var listDirectionTraining = await _versionsDirectionTrainingRepository.GetAllAsync();

                // Проверяем на null
                if (listTrainingAreas == null || listDirectionTraining == null)
                {
                    _logger.LogWarning("Список областей или направлений обучения пуст.");
                    return listTrainingAreas ?? new List<TrainingArea>();
                }
                // Перебираем каждую область обучения
                foreach (var area in listTrainingAreas)
                {
                    // Перебираем направления обучения
                    foreach (var directionTraining in listDirectionTraining)
                    {
                        // Сравниваем первые два символа кодов
                        if (area.Code.Length >= 2 && directionTraining.Code.Length >= 2 &&
                            area.Code.Substring(0, 2) == directionTraining.Code.Substring(0, 2))
                        {
                            // Если совпадают, добавляем направление в область
                            area.TrainingDirections.Add(directionTraining);
                        }
                    }
                }

                _logger.LogInformation("Успешно завершен процесс получения и сортировки всех направлений обучения.");
                // Возвращаем результат
                return listTrainingAreas;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении и сортировки всех направлений обучения.");
                return null; // Возвращаем null в случае ошибки
            }

        }
    }
}