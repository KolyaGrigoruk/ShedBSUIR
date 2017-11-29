using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using BSUIR_Scheduler.Models;

namespace BSUIR_Scheduler
{
    class StoreService
    {
        public async Task<ScheduleInfo> ReadJsonScheduleInfo()
        {
            ScheduleInfo scheduleInfo = new ScheduleInfo();
            var serializer = new DataContractJsonSerializer(typeof(ScheduleInfo));
            bool existed = await FileIsExist(ApplicationData.Current.LocalFolder, "scheduleInfo.json");
            if (existed)
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("scheduleInfo.json"))
                {
                    scheduleInfo = (ScheduleInfo)serializer.ReadObject(stream);
                }
            }
            return scheduleInfo;
        }

        public async Task<bool> FileIsExist(StorageFolder folder, string fileName)
        {
            return (await folder.GetFilesAsync()).Any(x => x.Name == fileName);
        }

        public async Task<Settings> ReadJsonSettingsAsync(string fileName)
        {
            Settings settings = new Settings();
            var serializer = new DataContractJsonSerializer(typeof(Settings));

            bool existed = await FileIsExist(ApplicationData.Current.LocalFolder, fileName);
            if (existed)
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
                {
                    settings = (Settings)serializer.ReadObject(stream);
                }
            }
          
            return settings;
        }
        public async Task WriteJsonScheduleInfoAsync(ScheduleInfo scheduleInfo,string fileName)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(ScheduleInfo));
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                    fileName, CreationCollisionOption.ReplaceExisting))
                {
                    serializer.WriteObject(stream, scheduleInfo);
                }
            }
            catch (Exception e)
            {
                await new MessageDialog("Произошла ошибка при сохранении расписания." + e.ToString()).ShowAsync();
            }
        }
        public async Task WriteJsonSettingsAsync(Settings settings)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(Settings));
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                    "data.json", CreationCollisionOption.ReplaceExisting))
                {
                    serializer.WriteObject(stream, settings);
                }
            }
            catch (Exception e)
            {
                await new MessageDialog("Произошла ошибка при сохранении настроек." + e.ToString()).ShowAsync();
            }
        }

    }
}
