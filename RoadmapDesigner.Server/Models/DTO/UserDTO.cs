using System;

namespace RoadmapDesigner.Server.Models.DTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; } // Уникальный идентификатор пользователя

        public string LastName { get; set; } = null!; // Фамилия

        public string FirstName { get; set; } = null!; // Имя

        public string? MiddleName { get; set; } // Отчество (опционально)

        public string Login { get; set; } = null!; // Логин

        public string Email { get; set; } = null!; // Email

        public string Role { get; set; } // Роль пользователя
    }
}