using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using BSUIR_Scheduler.Models;
using Newtonsoft.Json;

namespace BSUIR_Scheduler.Services
{
    class HttpService
    {
        string GET_SCHEDULE_ST = "http://students.bsuir.by/api/v1/studentGroup/schedule";
        public async Task<ScheduleInfo> GetSchedulesFromServer(string groupId)
        {
            ScheduleInfo scheduleInfo = new ScheduleInfo();
            HttpClient httpClient = new HttpClient();
            try
            {
                string result = await httpClient.GetStringAsync(this.GET_SCHEDULE_ST + "?studentGroup=" + groupId);
                scheduleInfo = JsonConvert.DeserializeObject<ScheduleInfo>(result);
            }
            catch (Exception e)
            {
                await new MessageDialog("Проблемы с подключением к интернету или с сайтом bsuir.by." + e.ToString()).ShowAsync();
            }
            httpClient.Dispose();
            return scheduleInfo;
        }
    }
}
