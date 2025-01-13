namespace RoadmapDesigner.Server.Models.DTO
{
    public class VersionsDirectionTrainingDTO
    {
        public Guid Uuid { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string LevelQualification { get; set; } = null!;

        public string FormEducation { get; set; } = null!;

        public string TrainingDepartment { get; set; } = null!;

        public DateOnly CreatedDate { get; set; }

    }
}
