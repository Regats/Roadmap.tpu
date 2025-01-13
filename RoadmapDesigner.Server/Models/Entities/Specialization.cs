using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entities;

public partial class Specialization
{
    public Guid Uuid { get; set; }

    public Guid VersionsDirectionTrainningUuid { get; set; }

    public string Name { get; set; } = null!;

    public string? RoadmapJson { get; set; }

    public virtual ICollection<DisciplinesSpecialization> DisciplinesSpecializations { get; set; } = new List<DisciplinesSpecialization>();

    public virtual VersionsDirectionTraining VersionsDirectionTrainningUu { get; set; } = null!;
}
