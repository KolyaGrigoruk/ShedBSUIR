namespace BSUIR_Scheduler.Models
{
    public class ScheduleItem
    {
        public int[] WeekNumber { get; set; }
        public string[] StudentGroup { get; set; }
        public string[] StudentGroupInformation { get; set; }
        public int NumSubgroup { get; set; }
        public string[] Auditory { get; set; }
        public string LessonTime { get; set; }
        public string StartLessonTime { get; set; }
        public string EndLessonTime { get; set; }
        public string Subject { get; set; }
        public string Note { get; set; }
        public string LessonType { get; set; }
        public Employee[] Employee { get; set; }
        public string StudentGroupModelList { get; set; }
        public bool Zaoch { get; set; }
    }
}

