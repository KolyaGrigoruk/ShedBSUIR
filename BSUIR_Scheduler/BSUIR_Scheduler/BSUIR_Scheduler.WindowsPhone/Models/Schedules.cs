using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BSUIR_Scheduler.Models
{
    public class Schedules : INotifyPropertyChanged
    {
        private string _weekDay;
        private ScheduleItem[] _schedule;

        public string WeekDay
        {
            get { return _weekDay; }
            set
            {
                _weekDay = value;
                OnPropertyChanged("WeekDay");
            }
        }

        public ScheduleItem[] Schedule
        {
            get { return _schedule; }
            set
            {
                _schedule = value;
                OnPropertyChanged("Schedule");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
