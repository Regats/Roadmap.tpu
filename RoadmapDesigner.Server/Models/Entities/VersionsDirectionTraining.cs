using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entities;

public partial class VersionsDirectionTraining
{
    public Guid Uuid { get; set; }

    public string Code { get; set; } = null!;

    public string LevelQualification { get; set; } = null!;

    public string FormEducation { get; set; } = null!;

    public string TrainingDepartment { get; set; } = null!;

    public DateOnly CreatedDate { get; set; }

    public virtual DirectionTraining CodeNavigation { get; set; } = null!;

    public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();

    public virtual ICollection<VersionsDirectionTrainingUser> VersionsDirectionTrainingUsers { get; set; } = new List<VersionsDirectionTrainingUser>();
}
