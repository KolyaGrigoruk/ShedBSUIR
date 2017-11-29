using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BSUIR_Scheduler.Models
{
    public class Settings : INotifyPropertyChanged
    {
        private string _groupId;
        private int _subGroup;
        private int _week;
        private string _role;

        public string GroupId
        {
            get { return _groupId; }
            set
            {
                _groupId = value;
                OnPropertyChanged("GroupId");
            }
        }

        public int SubGroup
        {
            get { return _subGroup; }
            set
            {
                _subGroup = value;
                OnPropertyChanged("SubGroup");
            }
        }

        public int Week
        {
            get { return _week; }
            set
            {
                _week = value;
                OnPropertyChanged("Week");
            }
        }

        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged("Role");
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