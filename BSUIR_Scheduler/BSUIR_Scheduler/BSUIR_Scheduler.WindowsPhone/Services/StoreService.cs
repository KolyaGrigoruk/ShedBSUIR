using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using BSUIR_Scheduler.Models;

namespace BSUIR_Scheduler
{
    class StoreService
    {
        public async Task<bool> FileIsExist(StorageFolder folder, string fileName)
        {
            return (await folder.GetFilesAsync()).Any(x => x.Name == fileName);
        }

        public async Task Write<T>(T dataObject,string fileName)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                    fileName, CreationCollisionOption.ReplaceExisting))
                {
                    serializer.WriteObject(stream, dataObject);
                }
            }
            catch (Exception e)
            {
                await new MessageDialog("Произошла ошибка при сохранении данных." + e.StackTrace).ShowAsync();
            }
        }

        public async Task<T> Read<T>(string fileName) where T : new()
        {    
            var serializer = new DataContractJsonSerializer(typeof(T));
            bool existed = await FileIsExist(ApplicationData.Current.LocalFolder, fileName);
            if (existed)
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
                {
                    return (T)serializer.ReadObject(stream);
                }
            }
            return new T();
        }
    }
}
