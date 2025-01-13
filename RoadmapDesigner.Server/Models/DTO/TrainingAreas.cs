namespace RoadmapDesigner.Server.Models.DTO
{
    public static class TrainingAreas
    {
        public static List<TrainingArea> GetTrainingAreas()
        {
            return new List<TrainingArea>
            {
                new TrainingArea
                {
                    Code = "01.00.00",
                    Name = "МАТЕМАТИКА И МЕХАНИКА"
                },
                new TrainingArea
                {
                    Code = "02.00.00",
                    Name = "КОМПЬЮТЕРНЫЕ И ИНФОРМАЦИОННЫЕ НАУКИ"
                },
                new TrainingArea
                {
                    Code = "03.00.00",
                    Name = "ФИЗИКА И АСТРОНОМИЯ"
                },
                new TrainingArea
                {
                    Code = "04.00.00",
                    Name = "ХИМИЯ"
                },
                new TrainingArea
                {
                    Code = "05.00.00",
                    Name = "НАУКИ О ЗЕМЛЕ"
                },
                new TrainingArea
                {
                    Code = "06.00.00",
                    Name = "БИОЛОГИЧЕСКИЕ НАУКИ"
                },
                new TrainingArea
                {
                    Code = "q",
                    Name = "q"
                },
                new TrainingArea
                {
                    Code = "q",
                    Name = "q"
                },
            };
        }
    }
}
