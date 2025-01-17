using System;

namespace RoadmapDesigner.Server.Models.DTO
{
    public class SpecializationDTO
    {
        public Guid Uuid { get; set; } // Уникальный идентификатор специализации

        public string Name { get; set; } = null!; // Название специализации

        public string? RoadmapJson { get; set; } // JSON-строка с данными дорожной карты
    }
}