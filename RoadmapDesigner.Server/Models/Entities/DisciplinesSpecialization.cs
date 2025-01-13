using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entities;

public partial class DisciplinesSpecialization
{
    public Guid Uuid { get; set; }

    public Guid DisciplinesUuid { get; set; }

    public Guid SpecializationUuid { get; set; }

    public virtual Discipline DisciplinesUu { get; set; } = null!;

    public virtual Specialization SpecializationUu { get; set; } = null!;
}
