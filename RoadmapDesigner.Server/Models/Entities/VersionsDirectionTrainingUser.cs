using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entities;

public partial class VersionsDirectionTrainingUser
{
    public Guid Uuid { get; set; }

    public Guid VersionsDirectionTrainningUuid { get; set; }

    public Guid UsersUserId { get; set; }

    public virtual User UsersUser { get; set; } = null!;

    public virtual VersionsDirectionTraining VersionsDirectionTrainningUu { get; set; } = null!;
}
