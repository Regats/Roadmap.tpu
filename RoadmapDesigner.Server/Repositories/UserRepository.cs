using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Models.Entities;

namespace RoadmapDesigner.Server.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly RoadmapContext _context; // Контекст базы данных
        private readonly ILogger<DirectionTrainingRepository> _logger; // Логгер для записи информации и ошибок

        // Конструктор, принимающий контекст и логгер
        public UserRepository(RoadmapContext context, ILogger<DirectionTrainingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userID)
        {
            var user = await _context.Users.FindAsync(userID);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDTO> GetUserByGuidAsync(Guid userID)
        {
            try
            {
                var user = await _context.Users.FindAsync(userID);
                if (user == null)
                {
                    return null; // Или можно выбросить исключение, если пользователь не найден
                }

                var role = await _context.UserRoles.FindAsync(user.RoleId);

                return new UserDTO
                {
                    UserId = user.UserId,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    Email = user.Email,
                    Login = user.Login,
                    Role = role?.ToString() // Проверка на null для роли
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неожиданная ошибка."); // Логирование неожиданной ошибки
                return null; // Возврат null в случае ошибки
            }
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Role) // Загрузка связанных ролей
                    .ToListAsync();

                var userDtos = users.Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    Login = u.Login,
                    Email = u.Email,
                    Role = u.Role.Role
                }).ToList();

                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении пользователей."); // Логирование ошибки
                return new List<UserDTO>(); // Возврат пустого списка в случае ошибки
            }
        }


        public async Task UpdateUserAsync(Guid userID)
        {
            var user = _context.Users.Find(userID);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
