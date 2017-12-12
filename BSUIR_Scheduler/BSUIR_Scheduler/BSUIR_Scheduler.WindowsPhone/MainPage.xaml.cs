using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BSUIR_Scheduler.Models;
using BSUIR_Scheduler.Services;

namespace BSUIR_Scheduler
{
    public sealed partial class MainPage : Page
    {
        public const string SettingsFileName = "data.json";
        public const string ScheduleFileName = "scheduleInfo.json";
        public const string ScheduleEmployeeFileName = "scheduleEmployeeInfo.json";
        public const string EmployeesFileName = "employees.json";
        public const string EmployeeFileName = "employee.json";
        public const string AllGroupsFileName = "allgroups.json";
        public const string AllGroupsScheduleFileName = "allgroupsschedule.json";
        public const string AllEmployeesScheduleFileName = "allemployeesschedule.json";

        private readonly StoreService _storeService = new StoreService();
        private readonly HttpService _httpService = new HttpService();
        private readonly CalendarService _calendarService = new CalendarService(); 

        private Settings _settings = new Settings();
        private Schedules[] _schedules = new Schedules[1];
        private ScheduleInfo _scheduleInfo = new ScheduleInfo();

        public ObservableCollection<ScheduleItem> ListItem = new ObservableCollection<ScheduleItem>();
        public ObservableCollection<Schedules> ExamsList = new ObservableCollection<Schedules>();

        private string _currentDay = "";
        private string _nextDay = "";
        private string _prevDay = "";

        public MainPage()
        {
            InitializeComponent();
            InitSettings();
            InitDay();
            
            NavigationCacheMode = NavigationCacheMode.Required;
            DaySchedule.DataContext = ListItem;
            ExamsSchedule.DataContext = ExamsList;
            
            UpdateDayButton();           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitSettings();
            UpdateDayButton();
            GetSchedule();
            Button_Click(new object(), new RoutedEventArgs());
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_settings.Role == 0)
            {
                if (await _storeService.FileIsExist(ApplicationData.Current.LocalFolder, ScheduleFileName))
                {
                    _scheduleInfo = await _storeService.Read<ScheduleInfo>(ScheduleFileName);
                    GetSchedule();
                }
                else
                {
                    _scheduleInfo = await _httpService.GetSchedules(_settings.GroupId);
                    await _storeService.Write(_scheduleInfo, ScheduleFileName);
                    GetSchedule();
                }
            }
            else
            {
                var employee = await _storeService.Read<Employee>(EmployeeFileName);
                _scheduleInfo = await _httpService.GetEmployeeSchedule(employee.Id);
                await _storeService.Write(_scheduleInfo, ScheduleEmployeeFileName);
                GetSchedule();
            }
                
        }

        public async Task GetSchedule()
        {
            if (_settings.Role == 1)
            {
                _scheduleInfo = await _storeService.Read<ScheduleInfo>(ScheduleEmployeeFileName);
                ListItem.Clear();
                _schedules = _scheduleInfo.Schedules;
                foreach (var schedule in _schedules)
                {
                    if (schedule.WeekDay.ToLower() == CurrentDay.Text)
                    {
                        foreach (var lesson in schedule.Schedule)
                        {
                            if (FilterLesson(lesson))
                            {
                                ListItem.Add(lesson);
                            }
                        }
                    }
                }
            }
            else
            {
                _scheduleInfo = await _storeService.Read<ScheduleInfo>(ScheduleFileName);
                ListItem.Clear();
                _schedules = _scheduleInfo.Schedules;
                foreach (var schedule in _schedules)
                {
                    if (schedule.WeekDay.ToLower() == CurrentDay.Text)
                    {
                        foreach (var lesson in schedule.Schedule)
                        {
                            if (FilterLesson(lesson))
                            {
                                ListItem.Add(lesson);
                            }
                        }
                    }
                }
            }
            
        }

        public bool FilterLesson(ScheduleItem lesson)
        {
            return (lesson.NumSubgroup == 0 || lesson.NumSubgroup == Subgroup.SelectedIndex + 1)
                   && (lesson.WeekNumber.Any(e => e == 0) || lesson.WeekNumber.Any(e => e == Week.SelectedIndex + 1));
        }
        public void InitDay()
        {
            string day = _currentDay.Length > 0
                ? _currentDay
                : _calendarService.TranslateDay(DateTime.Today.DayOfWeek);

            if (day == _calendarService.TranslateDay(DayOfWeek.Sunday))
            {
                _currentDay = _calendarService.TranslateDay(DayOfWeek.Monday);
                _nextDay = _calendarService.TranslateDay(DayOfWeek.Tuesday);
                _prevDay = _calendarService.TranslateDay(DayOfWeek.Saturday);
            }
            else if(day == _calendarService.TranslateDay(DayOfWeek.Saturday))
            {
                _currentDay = _calendarService.TranslateDay(DayOfWeek.Saturday);
                _nextDay = _calendarService.TranslateDay(DayOfWeek.Monday);
                _prevDay = _calendarService.TranslateDay(DayOfWeek.Friday);
            }
            else if (day == _calendarService.TranslateDay(DayOfWeek.Monday))
            {
                _currentDay = _calendarService.TranslateDay(DayOfWeek.Monday);
                _prevDay = _calendarService.TranslateDay(DayOfWeek.Saturday);
                _nextDay = _calendarService.TranslateDay(DayOfWeek.Tuesday);
            }
            else
            {
                _currentDay = _calendarService.TranslateDay(DateTime.Today.DayOfWeek);
                _nextDay = _calendarService.TranslateDay(DateTime.Today.DayOfWeek + 1);
                _prevDay = _calendarService.TranslateDay(DateTime.Today.DayOfWeek - 1);
            }
        }
        private async void InitSettings()
        { 
            var result = await _storeService.FileIsExist(ApplicationData.Current.LocalFolder, SettingsFileName);
            if (!result)
            {
                GoToSettingsPage(new object(), new RoutedEventArgs());
                return;
            }
            var settings = await _storeService.Read<Settings>(SettingsFileName);
            _settings.Role = settings.Role;
            _settings.Week = await _httpService.GetCurrentWeek() == 0 ? _settings.Week : await _httpService.GetCurrentWeek() - 1;
            if (_settings.Role == 1)
            {
                GroupID.Text = (await _storeService.Read<Employee>(EmployeeFileName)).Fio;
                RoleLabel.Text = "ФИО преподавателя";
                Week.SelectedIndex = _settings.Week;
                SubgroupPanel.Visibility = Visibility.Collapsed;
                return;
            }
           
            _settings.GroupId = settings.GroupId;
            _settings.SubGroup = settings.SubGroup;

            SubgroupPanel.Visibility = Visibility.Visible;

            GroupID.Text = _settings.GroupId;
            Week.SelectedIndex = _settings.Week;
            Subgroup.SelectedIndex = _settings.SubGroup;

            RoleLabel.Text = "Номер группы";

            await _storeService.Write(settings,SettingsFileName);
        }

        private void SettingsChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupID.Text != _settings.GroupId)
            {
                Button_Click(new object(), new RoutedEventArgs());
                return;
            }
            GetSchedule();
        }

        private void NextDayClick(object sender, RoutedEventArgs e)
        {
            _prevDay = _currentDay;
            _currentDay = _nextDay;
            _nextDay = _calendarService.NextDay(_nextDay);
            UpdateDayButton();
            GetSchedule();
        }

        private void PrevDay_Click(object sender, RoutedEventArgs e)
        {
            _nextDay = _currentDay;
            _currentDay = _prevDay;
            _prevDay = _calendarService.PrevDay(_prevDay);
            UpdateDayButton();
            GetSchedule();
        }

        private void UpdateDayButton()
        {
            PrevDay.Content = _prevDay;
            CurrentDay.Text = _currentDay;
            NextDay.Content = _nextDay;
        }

        private void GoToSettingsPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private async void GetExams()
        {
            if (_settings.Role == 1)
            {
                _scheduleInfo = await _storeService.Read<ScheduleInfo>(ScheduleEmployeeFileName);
            }
            else
            {
                _scheduleInfo = await _storeService.Read<ScheduleInfo>(ScheduleFileName);
            }

            ExamsList.Clear();
            _schedules = _scheduleInfo.ExamSchedules;
            foreach (var schedule in _schedules)
            {
                ExamsList.Add(schedule);
            }
        }

        private void ExamsButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (examsButton.Content != null && examsButton.Content.Equals("Экзамены"))
            {
                GetExams();
                DaySchedule.Visibility = Visibility.Collapsed;
                PrevDay.Visibility = Visibility.Collapsed;
                CurrentDay.Visibility = Visibility.Collapsed;
                NextDay.Visibility = Visibility.Collapsed;
                SubgroupPanel.Visibility = Visibility.Collapsed;
                WeekNumberPanel.Visibility = Visibility.Collapsed;
                examsButton.Content = "Пары";
            }
            else
            {
                DaySchedule.Visibility = Visibility.Visible;
                PrevDay.Visibility = Visibility.Visible;
                CurrentDay.Visibility = Visibility.Visible;
                NextDay.Visibility = Visibility.Visible;
                SubgroupPanel.Visibility = Visibility.Visible;
                WeekNumberPanel.Visibility = Visibility.Visible;
                examsButton.Content = "Экзамены";
            }         
        }
    }
}
