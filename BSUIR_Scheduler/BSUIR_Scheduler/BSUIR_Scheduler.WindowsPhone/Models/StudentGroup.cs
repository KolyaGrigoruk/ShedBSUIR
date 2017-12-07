namespace BSUIR_Scheduler.Models
{
    public class StudentGroup
    {
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
        public int SpecialityDepartmentEducationFormId { get; set; }
        public string SpecialityName { get; set; }
        public string Course { get; set; }
        public int Id { get; set; }
        public string CalendarId { get; set; }
    }
}
