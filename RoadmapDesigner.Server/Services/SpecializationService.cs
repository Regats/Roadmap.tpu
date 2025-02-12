using Microsoft.Extensions.Logging;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapDesigner.Server.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationRepository _specializationRepository; // Репозиторий для работы со специализациями
        private readonly ILogger<SpecializationService> _logger;  // Логгер для записи информации и ошибок

        // Конструктор для внедрения зависимостей
        public SpecializationService(ISpecializationRepository specializationRepository, ILogger<SpecializationService> logger)
        {
            _logger = logger;  // Инициализация логгера
            _specializationRepository = specializationRepository; // Инициализация репозитория
        }


        // Метод для асинхронного получения списка специализаций по идентификатору направления подготовки
        public async Task<List<SpecializationDTO>> GetListSpecializationsByDirTrainingUuid(Guid dirTrainingUuid)
        {
            try
            {
                _logger.LogInformation($"Начало процесса получения списка специализаций для направления подготовки с UUID: {dirTrainingUuid}");

                // Вызов метода репозитория
                var listSpecialization = await _specializationRepository.GetListSpecializationsByDirTrainingUuid(dirTrainingUuid);

                _logger.LogInformation($"Успешно получен список специализаций для направления подготовки с UUID: {dirTrainingUuid}");
                return listSpecialization;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении списка специализаций для направления подготовки с UUID: {dirTrainingUuid}");
                return null;   // Возвращаем null в случае ошибки
            }
        }

        // Метод для асинхронного получения специализации по UUID
        public async Task<SpecializationDTO> GetSpecializationByUuid(Guid specUuid)
        {
            try
            {
                _logger.LogInformation($"Начало процесса получения специализации с UUID: {specUuid}");

                // Вызов метода репозитория
                var specialization = await _specializationRepository.GetSpecializationByUuid(specUuid);

                _logger.LogInformation($"Успешно получена специализация с UUID: {specUuid}");
                return specialization;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при получении специализации с UUID: {specUuid}");
                return null;  // Возвращаем null в случае ошибки
            }
        }

        public async Task<bool> UpdateSpecializationAsync(SpecializationDTO specializationDTO)
        {
            try
            {
                _logger.LogInformation($"Начало обновления специализации с UUID: {specializationDTO.Uuid}");

                // Вызываем репозиторий для обновления специализации
                bool result = await _specializationRepository.UpdateSpecializationAsync(specializationDTO);

                if (result)
                {
                    _logger.LogInformation($"Специализация с UUID: {specializationDTO.Uuid} успешно обновлена.");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Не удалось обновить специализацию с UUID: {specializationDTO.Uuid}.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Произошла ошибка при обновлении специализации с UUID: {specializationDTO.Uuid}");
                return false; // Возвращаем false в случае ошибки
            }
        }
    }
}