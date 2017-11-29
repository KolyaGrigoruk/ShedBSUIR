using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BSUIR_Scheduler.Models
{
    public class Employee
    {
        private string _firstName;
        private string _lastName;
        private string _middleName;
        private string _rank;
        private string _photoLink;
        private string _calendarId;
        private string[] _academicDepartment;
        private int _id;
        private string _fio;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }

        public string Rank
        {
            get { return _rank; }
            set
            {
                _rank = value;
                OnPropertyChanged("Rank");
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string PhotoLink
        {
            get { return _photoLink; }
            set
            {
                _photoLink = value;
                OnPropertyChanged("PhotoLink");
            }
        }

        public string CalendarId
        {
            get { return _calendarId; }
            set
            {
                _calendarId = value;
                OnPropertyChanged("CalendarId");
            }
        }

        public string[] AcademicDepartment
        {
            get { return _academicDepartment; }
            set
            {
                _academicDepartment = value;
                OnPropertyChanged("AcademicDepartment");
            }
        }

        public string Fio
        {
            get { return _fio; }
            set
            {
                _fio = value;
                OnPropertyChanged("Fio");
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