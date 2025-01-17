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
    // Репозиторий для работы с направлениями обучения
    public class DirectionTrainingRepository : IDirectionTrainingRepository
    {
        private readonly RoadmapContext _context; // Контекст базы данных
        private readonly ILogger<DirectionTrainingRepository> _logger; // Логгер для записи информации и ошибок

        // Конструктор, принимающий контекст и логгер
        public DirectionTrainingRepository(RoadmapContext context, ILogger<DirectionTrainingRepository> logger)
        {
            _context = context; // Инициализация контекста
            _logger = logger;   // Инициализация логгера
        }

        // Метод для получения всех версий направлений обучения
        public async Task<IEnumerable<VersionsDirectionTrainingDTO>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Начало выполнения запроса на получение всех версий направлений обучения.");

                // Получение списка версий направлений обучения с загрузкой связанных данных из DirectionTraining
                var listDirectionTrainings = await _context.VersionsDirectionTrainings
                    .Include(v => v.CodeNavigation) // Загрузка данных из DirectionTraining
                    .ToListAsync();

                _logger.LogInformation($"Запрос на получение всех версий направлений обучения вернул {listDirectionTrainings.Count} записей.");

                // Преобразование данных в DTO и возврат результата
                return listDirectionTrainings.Select(dt => new VersionsDirectionTrainingDTO
                {
                    Uuid = dt.Uuid,
                    Code = dt.Code,
                    Name = dt.CodeNavigation.Name,
                    LevelQualification = dt.LevelQualification,
                    FormEducation = dt.FormEducation,
                    TrainingDepartment = dt.TrainingDepartment,
                    CreatedDate = dt.CreatedDate,
                });
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

        // Метод для получения деталей определённой версии направления обучения по UUID
        public async Task<VersionsDirectionTrainingDTO> GetDetailsAsync(Guid uuid)
        {
            _logger.LogInformation($"Начало выполнения запроса на получение деталей версии направления обучения с UUID: {uuid}.");
            try
            {
                // Получение последней версии направления обучения по UUID, с загрузкой данных из DirectionTraining
                var version = await _context.VersionsDirectionTrainings
                     .Include(v => v.CodeNavigation)
                    .Where(v => v.Uuid == uuid) // Фильтрация по UUID
                     .OrderByDescending(v => v.CreatedDate) // Сортировка по дате создания (последняя)
                    .FirstOrDefaultAsync(); // Получение первой или null

                // Проверка на null
                if (version == null)
                {
                    _logger.LogWarning($"Версия направления обучения с UUID: {uuid} не найдена.");
                    return null; // Возврат null если версия не найдена
                }

                _logger.LogInformation($"Запрос на получение деталей версии направления обучения с UUID: {uuid} успешен.");

                // Преобразование данных в DTO и возврат результата
                return new VersionsDirectionTrainingDTO
                {
                    Uuid = version.Uuid,
                    Code = version.Code,
                    Name = version.CodeNavigation.Name,
                    LevelQualification = version.LevelQualification,
                    FormEducation = version.FormEducation,
                    TrainingDepartment = version.TrainingDepartment,
                    CreatedDate = version.CreatedDate,
                };
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных.");
                throw;  // Пробрасываем исключение на уровень сервиса
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неожиданная ошибка.");
                throw; // Пробрасываем исключение на уровень сервиса
            }
        }
    }
}