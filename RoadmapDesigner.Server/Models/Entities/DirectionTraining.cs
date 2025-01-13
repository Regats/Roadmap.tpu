using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entities;

public partial class DirectionTraining
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<VersionsDirectionTraining> VersionsDirectionTrainings { get; set; } = new List<VersionsDirectionTraining>();
}
