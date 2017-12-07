namespace BSUIR_Scheduler.Models
{

    public class ScheduleInfo
    {
        public Employee Employee { get; set; }
        public StudentGroup StudentGroup { get; set; }
        public Schedules[] Schedules { get; set; }
        public Schedules[] ExamSchedules { get; set; }
        public string TodayDate { get; set; }
        public TodaySchedules[] TodaySchedules { get; set; }
        public string TomorrowDate { get; set; }
        public TomorrowSchedules[] TomorrowSchedules { get; set; }
        public int CurrentWeekNumber { get; set; }
    }
}
