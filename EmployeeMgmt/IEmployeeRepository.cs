using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMgmt
{
    public interface IEmployeeRepository
    {
        Task<HttpResponseMessage> GetEmployees(string url);
        Task<HttpResponseMessage> GetEmployeebyID(string url);
        Task<HttpResponseMessage> CreateEmployee<T>(string url, T model) where T : class;
        Task<HttpResponseMessage> UpdateEmployee<T>(string url, T model) where T : class;
        Task<HttpResponseMessage> DeleteEmployee(string url);
    }
}
