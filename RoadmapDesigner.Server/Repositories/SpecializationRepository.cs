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
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly RoadmapContext _context; // Контекст базы данных
        private readonly ILogger<SpecializationRepository> _logger; // Логгер для записи информации и ошибок

        // Конструктор для внедрения зависимостей
        public SpecializationRepository(RoadmapContext context, ILogger<SpecializationRepository> logger)
        {
            _context = context;  // Инициализация контекста
            _logger = logger;  // Инициализация логгера
        }

        // Метод для асинхронного получения списка специализаций по идентификатору направления подготовки
        public async Task<List<SpecializationDTO>> GetListSpecializationsByDirTrainingUuid(Guid dirTrainingUuid)
        {
            try
            {
                _logger.LogInformation($"Начало запроса на получение списка специализаций для направления подготовки с UUID: {dirTrainingUuid}");

                // Получаем список специализаций из базы данных
                var listSpecializations = await _context.Specializations
                     .Where(s => s.VersionsDirectionTrainningUuid == dirTrainingUuid) // Фильтрация по UUID направления подготовки
                     .ToListAsync()
                     .ConfigureAwait(false);

                _logger.LogInformation($"Запрос на получение списка специализаций для направления подготовки с UUID: {dirTrainingUuid} вернул {listSpecializations.Count} записей.");

                // Преобразуем список специализаций в список DTO
                var listSpecializationDTO = listSpecializations.Select(s => new SpecializationDTO
                {
                    Uuid = s.Uuid,  // Копируем идентификатор
                    Name = s.Name,  // Копируем название
                    RoadmapJson = s.RoadmapJson, // Копируем JSON-строку
                }).ToList();
                return listSpecializationDTO;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных.");
                throw; // Пробрасываем исключение на уровень сервиса
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неожиданная ошибка.");
                throw; // Пробрасываем исключение на уровень сервиса
            }
        }

        // Метод для асинхронного получения специализации по UUID
        public async Task<SpecializationDTO> GetSpecializationByUuid(Guid specUuid)
        {
            try
            {
                _logger.LogInformation($"Начало запроса на получение специализации с UUID: {specUuid}");

                // Получаем специализацию из базы данных по UUID
                var specialization = await _context.Specializations
                    .FindAsync(specUuid)
                     .ConfigureAwait(false);


                // Проверка на null
                if (specialization == null)
                {
                    _logger.LogWarning($"Специализация с UUID: {specUuid} не найдена.");
                    return null;
                }

                _logger.LogInformation($"Запрос на получение специализации с UUID: {specUuid} успешен.");

                // Преобразуем специализацию в DTO
                return new SpecializationDTO
                {
                    Uuid = specialization.Uuid,   // Копируем идентификатор
                    Name = specialization.Name, // Копируем название
                    RoadmapJson = specialization.RoadmapJson,  // Копируем JSON-строку
                };
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных.");
                throw;   // Пробрасываем исключение на уровень сервиса
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неожиданная ошибка.");
                throw;  // Пробрасываем исключение на уровень сервиса
            }
        }
    }
}