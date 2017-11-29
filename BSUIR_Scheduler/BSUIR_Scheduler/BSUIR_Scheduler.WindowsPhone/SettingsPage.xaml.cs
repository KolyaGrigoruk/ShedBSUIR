using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows;
using Windows.UI.Popups;
using BSUIR_Scheduler.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BSUIR_Scheduler.Models;
using System.Runtime.Serialization;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BSUIR_Scheduler
{
    public sealed partial class SettingsPage : Page
    {
        private Settings _settings = new Settings();
        private StoreService _storeService = new StoreService();

        public SettingsPage()
        {
            this.InitializeComponent();
            getSettings();
        }

        public async void getSettings()
        {
            bool result = await _storeService.FileIsExist(ApplicationData.Current.LocalFolder, "data.json");
           // if (result)
            //{
              //  Frame.Navigate(typeof(MainPage));
            //}
        }
      
        private void ButtonGetSchedule(object sender, RoutedEventArgs e)
        {
            this._settings.GroupId = GroupID.Text;
            _storeService.WriteJsonSettingsAsync(_settings);
            Frame.Navigate(typeof(MainPage), this._settings);
        }

        public int Validate()
        {
           return 0;   
        }
    }
}
