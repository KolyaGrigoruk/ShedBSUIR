using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BSUIR_Scheduler.Models;
using Windows.Storage;
using Windows.UI.Popups;
using BSUIR_Scheduler.Services;

namespace BSUIR_Scheduler
{
    public sealed partial class SettingsPage : Page
    {
        private Settings _settings = new Settings();
        private StoreService _storeService = new StoreService();
        private HttpService _httpService = new HttpService();

        private List<Employee> _employees { get; set; }
        public ObservableCollection<Employee> suggestList = new ObservableCollection<Employee>();

        public SettingsPage()
        {
            InitializeComponent();
            studentButton.IsChecked = true;
            ChangedSettings(new object(), new RoutedEventArgs());
            _employees = new List<Employee>();
            InitEmployees();
            Suggestions.ItemsSource = suggestList;
        }

        private void ButtonGetSchedule(object sender, RoutedEventArgs e)
        {
            if ((bool) studentButton.IsChecked)
            {
                _settings.GroupId = GroupID.Text;
                _settings.Role = 0;
                _storeService.Write(_settings, MainPage.SettingsFileName);
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                _settings.Role = 1;
                _storeService.Write(_settings, MainPage.SettingsFileName);
                _storeService.Write(suggestList.First(), MainPage.EmployeeFileName);
                Frame.Navigate(typeof(MainPage));
            }
        }

        public async void InitEmployees()
        {
            try
            {
                if (await _storeService.FileIsExist(ApplicationData.Current.LocalFolder, MainPage.EmployeesFileName))
                {
                    _employees = await _storeService.Read<List<Employee>>(MainPage.EmployeesFileName);
                    return;
                }
                List<Employee> response = await _httpService.GetEmployees();
                await _storeService.Write(response, MainPage.EmployeesFileName);
                _employees = response;

            }
            catch (Exception e)
            {
                await new MessageDialog("Произошла ошибка, повторите операцию позже.").ShowAsync();
            }
        }


        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                suggestList.Clear();
                suggestList.Add(searchEmployee(sender.Text));
            }
        }
        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Suggestions.Text = suggestList.First().Fio; 
        }

        private Employee searchEmployee(string data)
        {
            var result = new Employee();
            foreach (var e in _employees)
            {
                if (e.Fio.StartsWith(data))
                {
                    result = e;
                    break;
                }    
            }
            return result;
        }

        private void ChangedSettings(object sender, RoutedEventArgs e)
        {
            if (!(bool) studentButton.IsChecked || (bool) teacherButton.IsChecked)
            {
                GroupID.Visibility = Visibility.Collapsed;
                Suggestions.Visibility = Visibility.Visible;
                Title.Text = "Введите ФИО преподавателя";               
            }
            else
            {
                Title.Text = "Введите Вашу группу";
                GroupID.Visibility = Visibility.Visible;
                Suggestions.Visibility = Visibility.Collapsed;
            }
        }
        public int Validate()
        {
           return 0;   
        }
    }
}
