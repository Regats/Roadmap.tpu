using Microsoft.Extensions.Logging;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Services
{
    public class DisciplineService : IDisciplineService
    {
        private readonly IDisciplineRepository _disciplineRepository;  // Репозиторий для работы с дисциплинами
        private readonly ILogger<DisciplineService> _logger; // Логгер для записи информации и ошибок

        // Конструктор для внедрения зависимостей
        public DisciplineService(IDisciplineRepository disciplineRepository, ILogger<DisciplineService> logger)
        {
            _disciplineRepository = disciplineRepository;  // Инициализация репозитория
            _logger = logger;  // Инициализация логгера
        }

        // Метод для асинхронного получения списка дисциплин
        public async Task<List<DisciplineDTO>> GetListDisciplinesAsync()
        {
            try
            {
                _logger.LogInformation("Начало процесса получения списка дисциплин.");
                // Вызов метода репозитория для получения списка дисциплин
                var listDisciplines = await _disciplineRepository.GetListDisciplinesAsync();

                _logger.LogInformation("Успешно получен список дисциплин.");

                return listDisciplines; // Возвращаем список DTO
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении списка дисциплин.");
                return null;  // Возвращаем null, если произошла ошибка
            }
        }
    }
}