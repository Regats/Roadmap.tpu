using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Repositories
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly RoadmapContext _context; // Контекст базы данных
        private readonly ILogger<DisciplineRepository> _logger; // Логгер для записи информации и ошибок

        public DisciplineRepository(RoadmapContext context, ILogger<DisciplineRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<DisciplineDTO>> GetListDisciplineAsync()
        {
            try
            {
                // Получаем список дисциплин из базы данных
                var listDiscipline = await _context.Disciplines.ToListAsync();

                // Преобразуем список дисциплин в список DTO
                var disciplineDTOs = listDiscipline.Select(d => new DisciplineDTO
                {
                    Uuid = d.Uuid,           // Предполагаем, что у Discipline есть свойство Id
                    Name = d.Name,       // Предполагаем, что у Discipline есть свойство Name
                                         // Добавьте другие свойства по мере необходимости
                }).ToList();

                return disciplineDTOs;
            }
            catch (Exception ex)
            {
                // Логирование исключения или обработка ошибки
                _logger.LogError(ex, "Ошибка при получении списка дисциплин");
                return null;
            }
        }
    }
}
