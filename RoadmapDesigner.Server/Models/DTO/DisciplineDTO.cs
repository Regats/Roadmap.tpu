using System;

namespace RoadmapDesigner.Server.Models.DTO
{
    public class DisciplineDTO
    {
        public Guid Uuid { get; set; } // Уникальный идентификатор дисциплины

        public string Name { get; set; } = null!; // Название дисциплины
    }
}