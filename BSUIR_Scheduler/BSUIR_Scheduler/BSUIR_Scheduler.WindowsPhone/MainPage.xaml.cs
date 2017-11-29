using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BSUIR_Scheduler;
using BSUIR_Scheduler.Models;
using BSUIR_Scheduler.Services;
using Newtonsoft.Json;

namespace BSUIR_Scheduler
{
    public sealed partial class MainPage : Page
    {
        private const string SettingsFileName = "data.json";
        private const string ScheduleFileName = "scheduleInfo.json";

        private readonly StoreService _storeService = new StoreService();
        private readonly HttpService _httpService = new HttpService();
        private readonly CalendarService _calendarService = new CalendarService(); 

        private Settings _settings = new Settings();
        private Schedules[] _schedules;
        private ScheduleInfo _scheduleInfo = new ScheduleInfo();

        public ObservableCollection<ScheduleItem> ListItem = new ObservableCollection<ScheduleItem>();

        private string _currentDay = "";
        private string _nextDay = "";
        private string _prevDay = "";

        public MainPage()
        {
            InitializeComponent();
            InitDay();
            NavigationCacheMode = NavigationCacheMode.Required;
            DaySchedule.DataContext = ListItem;
            
            UpdateDayButton();  
            Button_Click(new object(), new RoutedEventArgs());
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
           bool result = await _storeService.FileIsExist(ApplicationData.Current.LocalFolder, SettingsFileName);
            if (result)
            {
                var settings = await _storeService.ReadJsonSettingsAsync(SettingsFileName);

                _settings.Week = settings.Week;
                _settings.GroupId = settings.GroupId;
                _settings.SubGroup = settings.SubGroup;
                _settings.Role = settings.Role;
            }
            else
            {
                GoToSettingsPage(new object(), new RoutedEventArgs());
            }
        }
       

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _settings = await _storeService.ReadJsonSettingsAsync(SettingsFileName);
           
            GroupID.Text = _settings.GroupId;
            Week.SelectedIndex = _scheduleInfo.CurrentWeekNumber;
            Subgroup.SelectedIndex = _settings.SubGroup;

            bool existed = await _storeService.FileIsExist(ApplicationData.Current.LocalFolder, ScheduleFileName);
            if (existed)
            {
                _scheduleInfo = await _storeService.ReadJsonScheduleInfo();
                await GetSchedule();
            }
            else
            {
                _scheduleInfo = await _httpService.GetSchedulesFromServer(_settings.GroupId);
                await _storeService.WriteJsonScheduleInfoAsync(_scheduleInfo, ScheduleFileName);
                _scheduleInfo = await _storeService.ReadJsonScheduleInfo();
                await GetSchedule();
            }
        }

        public async Task GetSchedule()
        {
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

        public void InitWeekNumber()
        {
            
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

        private void SettingsChanged(object sender, SelectionChangedEventArgs e)
        {
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
    }
}
