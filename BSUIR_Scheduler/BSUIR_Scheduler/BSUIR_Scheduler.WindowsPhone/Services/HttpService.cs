using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Popups;
using BSUIR_Scheduler.Models;
using Newtonsoft.Json;

namespace BSUIR_Scheduler.Services
{
    class HttpService
    {
        private string API_SCHEDULE_STUDENT = "http://students.bsuir.by/api/v1/studentGroup/schedule";
        private string API_EMPLOYEES = "https://students.bsuir.by/api/v1/employees";
        private string API_EMPLOYEES_SCHEDULE = "https://students.bsuir.by/api/v1/portal/employeeSchedule";
        private string API_CURRENT_WEEK = "http://students.bsuir.by/api/v1/week";
        private string API_ALL_GROUPS = "https://students.bsuir.by/api/v1/groups";
        public async Task<ScheduleInfo> GetSchedules(string groupId)
        {
            ScheduleInfo scheduleInfo = new ScheduleInfo();
            HttpClient httpClient = new HttpClient();
            try
            {
                string result = await httpClient.GetStringAsync(this.API_SCHEDULE_STUDENT + "?studentGroup=" + groupId);
                scheduleInfo = JsonConvert.DeserializeObject<ScheduleInfo>(result);
            }
            catch (Exception e)
            {}
            httpClient.Dispose();
            return scheduleInfo;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            List<Employee> employeesList = new List<Employee>();
            HttpClient httpClient = new HttpClient();
            try
            {
                string result = await httpClient.GetStringAsync(API_EMPLOYEES);
                Employee[] employees = JsonConvert.DeserializeObject<Employee[]>(result);
                foreach (var item in employees)
                {
                    employeesList.Add(item);
                }
            }
            catch (Exception e)
            {
                await new MessageDialog("Проверьте соединение с интернетом.").ShowAsync();
            }
            httpClient.Dispose();
            return employeesList;
        }

        public async Task<ScheduleInfo> GetEmployeeSchedule(int employeeId)
        {
            ScheduleInfo employeeSchedule = new ScheduleInfo();
            HttpClient httpClient = new HttpClient();
            try
            {
                string result = await httpClient.GetStringAsync(API_EMPLOYEES_SCHEDULE + "?employeeId=" + employeeId);
                employeeSchedule = JsonConvert.DeserializeObject<ScheduleInfo>(result);
                return employeeSchedule;
            }
            catch (Exception e)
            {
                await new MessageDialog("Проверьте соединение с интернетом.").ShowAsync();
            }
            httpClient.Dispose();
            return employeeSchedule;
        }

        public async Task<List<StudentGroup>> GetAllGroups()
        {
            List<StudentGroup> listGroup = new List<StudentGroup>();
            HttpClient httpClient = new HttpClient();
            try
            {
                string result = await httpClient.GetStringAsync(API_ALL_GROUPS);
                StudentGroup[] studentGroups = JsonConvert.DeserializeObject<StudentGroup[]>(result);
                foreach (var studentGroup in studentGroups)
                {
                    listGroup.Add(studentGroup);
                }
                httpClient.Dispose();
                return listGroup;
            }
            catch (Exception e)
            {
                await new MessageDialog("Проверьте соединение с интернетом." + e).ShowAsync();
            }
            httpClient.Dispose();
            return listGroup;
        }

        public async Task<int> GetCurrentWeek()
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                return Convert.ToInt32(await httpClient.GetStringAsync(API_CURRENT_WEEK));
            }
            catch (Exception e)
            {}
            httpClient.Dispose();
            return 0;
        }
    }
}
