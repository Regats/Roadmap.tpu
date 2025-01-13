using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

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
            _context = context;
            _logger = logger;
        }

        // Метод для получения всех версий направлений обучения
        public async Task<IEnumerable<VersionsDirectionTrainingDTO>> GetAllVersionsDirectionTrainingAsync()
        {
            try
            {
                _logger.LogInformation("Функция GetAllDirectionTrainingAsync начала работу."); // Логирование начала работы метода

                // Получение списка направлений обучения с загрузкой связанных данных из DirectionTraining
                var listDirectionTrainings = await _context.VersionsDirectionTrainings
                    .Include(v => v.CodeNavigation) // Загрузка данных из DirectionTraining
                    .ToListAsync();

                _logger.LogInformation($"Функция GetAllDirectionTrainingAsync вернула {listDirectionTrainings.Count} объектов."); // Логирование количества полученных объектов

                // Преобразование данных в DTO и возврат результата
                return listDirectionTrainings.Select(dt => new VersionsDirectionTrainingDTO
                {
                    Uuid = dt.Uuid,
                    Code = dt.Code,
                    Name = dt.CodeNavigation.Name, // Данные из DirectionTraining
                    LevelQualification = dt.LevelQualification,
                    FormEducation = dt.FormEducation,
                    TrainingDepartment = dt.TrainingDepartment,
                    CreatedDate = dt.CreatedDate,
                });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных."); // Логирование ошибки базы данных
                return Enumerable.Empty<VersionsDirectionTrainingDTO>(); // Возврат пустого списка в случае ошибки
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неожиданная ошибка."); // Логирование неожиданной ошибки
                return Enumerable.Empty<VersionsDirectionTrainingDTO>(); // Возврат пустого списка в случае ошибки
            }
        }

        // Метод для получения деталей определённой версии направления обучения по UUID
        public async Task<VersionsDirectionTrainingDTO?> GetVersionDirectionTrainingDetailsAsync(Guid uuid)
        {
            _logger.LogInformation($"Функция GetVersionDirectionTrainingDetailsAsync начала работу с кодом {uuid}"); // Логирование начала работы метода с параметром UUID
            try
            {
                // Поиск записи по UUID и году создания, с загрузкой данных из DirectionTraining
                var version = await _context.VersionsDirectionTrainings
                    .Include(v => v.CodeNavigation) // Загрузка данных из DirectionTraining
                    .FirstOrDefaultAsync(dt => dt.Uuid == uuid && dt.CreatedDate.Year == DateTime.Now.Year);

                if (version == null) // Проверка, если запись не найдена
                {
                    _logger.LogWarning($"Версия с кодом {uuid} не найдена."); // Логирование предупреждения
                    return null; // Возврат null, если запись не найдена
                }

                _logger.LogInformation($"Функция GetVersionDirectionTrainingDetailsAsync вернула объект: {version} \nUUID: {version.Uuid}"); // Логирование успешного получения данных

                // Преобразование данных в DTO и возврат результата
                return new VersionsDirectionTrainingDTO
                {
                    Uuid = version.Uuid,
                    Code = version.Code,
                    Name = version.CodeNavigation.Name, // Данные из DirectionTraining
                    LevelQualification = version.LevelQualification,
                    FormEducation = version.FormEducation,
                    TrainingDepartment = version.TrainingDepartment,
                    CreatedDate = version.CreatedDate,
                };
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обращении к базе данных."); // Логирование ошибки базы данных
                return null; // Возврат null в случае ошибки
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неожиданная ошибка."); // Логирование неожиданной ошибки
                return null; // Возврат null в случае ошибки
            }
        }
    }
}

