using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entities;

public partial class User
{
    public Guid UserId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Login { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual UserRole Role { get; set; } = null!;

    public virtual ICollection<VersionsDirectionTrainingUser> VersionsDirectionTrainingUsers { get; set; } = new List<VersionsDirectionTrainingUser>();
}
