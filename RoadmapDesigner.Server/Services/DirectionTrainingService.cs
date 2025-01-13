using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;

namespace RoadmapDesigner.Server.Services
{
    public class DirectionTrainingService : IDirectionTrainingService
    {

        private readonly IDirectionTrainingRepository _versionsDirectionTrainingRepository;
        private readonly ILogger<DirectionTrainingService> _logger;

        public DirectionTrainingService(ILogger<DirectionTrainingService> logger, IDirectionTrainingRepository directionTrainingRepository) 
        {
            _logger = logger;
            _versionsDirectionTrainingRepository = directionTrainingRepository;
        }

        public async Task<VersionsDirectionTrainingDTO> GetDirectionTrainingDetails(Guid uuid)
        {
            var versionDirectionTraining = await _versionsDirectionTrainingRepository.GetVersionDirectionTrainingDetailsAsync(uuid);

            if (versionDirectionTraining == null) 
                return null;

            return versionDirectionTraining;
        }

        public async Task<List<TrainingArea>> SortAllDirectionTrainingToTrainingAreas()
        {
            // Получаем список областей обучения
            var listTrainingAreas = TrainingAreas.GetTrainingAreas();

            var listDirectionTraining = await _versionsDirectionTrainingRepository.GetAllVersionsDirectionTrainingAsync();

            // Проверяем на null
            if (listTrainingAreas == null || listDirectionTraining == null)
            {
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

            // Возвращаем результат как Task
            return listTrainingAreas;
        }
    }
}
