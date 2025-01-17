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
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly RoadmapContext _context; // Контекст базы данных
        private readonly ILogger<DisciplineRepository> _logger; // Логгер для записи информации и ошибок

        // Конструктор для внедрения зависимостей
        public DisciplineRepository(RoadmapContext context, ILogger<DisciplineRepository> logger)
        {
            _context = context; // Инициализация контекста
            _logger = logger;  // Инициализация логгера
        }

        // Метод для асинхронного получения списка дисциплин
        public async Task<List<DisciplineDTO>> GetListDisciplinesAsync()
        {
            try
            {
                _logger.LogInformation("Начало выполнения запроса на получение списка дисциплин.");

                // Получаем список дисциплин из базы данных
                var listDisciplines = await _context.Disciplines.ToListAsync();

                _logger.LogInformation($"Запрос на получение списка дисциплин вернул {listDisciplines.Count} записей.");

                // Преобразуем список дисциплин в список DTO
                var disciplineDTOs = listDisciplines.Select(d => new DisciplineDTO
                {
                    Uuid = d.Uuid,   // Копируем идентификатор
                    Name = d.Name,    // Копируем название
                }).ToList();
                return disciplineDTOs;
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
    }
}