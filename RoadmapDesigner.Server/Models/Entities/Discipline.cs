﻿using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entities;

public partial class Discipline
{
    public Guid Uuid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DisciplinesSpecialization> DisciplinesSpecializations { get; set; } = new List<DisciplinesSpecialization>();
}
