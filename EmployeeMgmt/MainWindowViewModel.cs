using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace EmployeeMgmt
{
    class MainWindowViewModel : BindableBase
    {
        #region Properties  
        public string API_URIs = ConfigurationManager.AppSettings["API_Uri"];

        private List<Employee> _employees;

        public List<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { SetProperty(ref _selectedEmployee, value); }
        }

        private bool _isLoadData;

        public bool IsLoadData
        {
            get { return _isLoadData; }
            set { SetProperty(ref _isLoadData, value); }
        }

        private string _responseMessage = "";

        public string ResponseMessage
        {
            get { return _responseMessage; }
            set { SetProperty(ref _responseMessage, value); }
        }

        #region [Create Employee Properties]  

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _gender;

        public string Gender
        {
            get { return _gender; }
            set { SetProperty(ref _gender, value); }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        #endregion
        private bool _isShowForm;

        public bool IsShowForm
        {
            get { return _isShowForm; }
            set { SetProperty(ref _isShowForm, value); }
        }

        private string _showPostMessage = "Fill the form to register an employee!";

        public string ShowPostMessage
        {
            get { return _showPostMessage; }
            set { SetProperty(ref _showPostMessage, value); }
        }

        private string _SearchInput;

        public string SearchInput
        {
            get { return _SearchInput; }
            set
            {
                SetProperty(ref _SearchInput, value);
            }

        }

        private IEmployeeRepository _repo;
        #endregion

        #region ICommands  
        public DelegateCommand GetButtonClicked { get; set; }
        public DelegateCommand ShowRegistrationForm { get; set; }
        public DelegateCommand SaveButtonClick { get; set; }
        public DelegateCommand<Employee> PutButtonClicked { get; set; }
        public DelegateCommand<Employee> DeleteButtonClicked { get; set; }
        public DelegateCommand SearchButtonClicked { get; set; }

        private string _progressStatus;

        public string ProgressStatus
        {
            get { return _progressStatus; }
            set { SetProperty(ref _progressStatus, value); }
        }
        #endregion

        #region Constructor  
        /// <summary>  
        /// Initalize perperies & delegate commands  
        /// </summary>  
        public MainWindowViewModel()
        {
            GetButtonClicked = new DelegateCommand(GetEmployeesDetails);
            PutButtonClicked = new DelegateCommand<Employee>(UpdateEmployeeDetails);
            DeleteButtonClicked = new DelegateCommand<Employee>(DeleteEmployeeDetails);
            SaveButtonClick = new DelegateCommand(CreateNewEmployee);
            ShowRegistrationForm = new DelegateCommand(RegisterEmployee);
            SearchButtonClicked = new DelegateCommand(SearchEmployee);
            _repo = Factory.Container.Resolve<IEmployeeRepository>();


        }
        #endregion


            #region CRUD  
            /// <summary>  
            /// Make visible Regiter user form  
            /// </summary>  
        private void RegisterEmployee()
        {
            IsShowForm = true;
        }

        /// <summary>  
        /// Fetches employee details  
        /// </summary>  
        private async void GetEmployeesDetails()
        {
            IsShowForm = false;
            ProgressStatus = "Loading Employees details";
            
            var employeeDetails =await _repo.GetEmployees(API_URIs);
            if (employeeDetails.StatusCode == HttpStatusCode.OK)
            {
                Employees = employeeDetails.Content.ReadAsAsync<List<Employee>>().Result;
                IsLoadData = true;
            }
            ProgressStatus = string.Empty;

        }

        /// <summary>  
        /// Adds new employee  
        /// </summary>  
        private async void CreateNewEmployee()
        {
            if (string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(Email)
                || string.IsNullOrEmpty(Gender)
                || string.IsNullOrEmpty(Status))
                return;
            Employee newEmployee = new Employee()
            {
                Name = Name,
                Email = Email,
                Gender = Gender,
                Status = Status
            };
            var employeeDetails =await _repo.CreateEmployee(API_URIs, newEmployee);
            if (employeeDetails.StatusCode == HttpStatusCode.Created)
            {
                ShowPostMessage = newEmployee.Name + "'s details has successfully been added!";
            }
            else
            {
                ShowPostMessage = "Failed to update " + newEmployee.Name + "'s details.";
            }

        }


        /// <summary>  
        /// Updates employee's record  
        /// </summary>  
        /// <param name="employee"></param>  
        private async void UpdateEmployeeDetails(Employee employee)
        {
            if (employee == null)
                return;
            ProgressStatus = "Updating Employee Details";
            var employeeDetails =await _repo.UpdateEmployee(API_URIs + employee.ID, employee);
            if (employeeDetails.StatusCode == HttpStatusCode.OK)
            {
                ResponseMessage = employee.Name + "'s details has successfully been updated!";
            }
            else
            {
                ResponseMessage = "Failed to update" + employee.Name + "'s details.";
            }
            ProgressStatus = string.Empty;
        }

        /// <summary>  
        /// Deletes employee's record  
        /// </summary>  
        /// <param name="employee"></param>  
        private async void DeleteEmployeeDetails(Employee employee)
        {
            ProgressStatus = "Deleting Employee Details";
            var employeeDetails = await _repo.DeleteEmployee(API_URIs + employee.ID);
            if (employeeDetails.StatusCode == HttpStatusCode.OK
                 || employeeDetails.StatusCode == HttpStatusCode.NoContent)
            {
                ProgressStatus = employee.Name + "'s details has successfully been deleted! reloading the employees details";
                GetEmployeesDetails();
            }
            else
            {
                ProgressStatus = "Failed to delete" + employee.Name + "'s details.";
            }
        }

        /// <summary>
        /// Search for employee by ID
        /// </summary>
        private async void SearchEmployee()
        {
            if (!string.IsNullOrEmpty(SearchInput))
            {
                ProgressStatus = "Searching for Employee by ID"+ SearchInput;
                IsLoadData = false;
                var employeeDetails = await _repo.GetEmployeebyID(API_URIs + SearchInput);
                if (employeeDetails.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Employees.Clear();
                    var temp = employeeDetails.Content.ReadAsAsync<Employee>().Result;
                    Employees = new List<Employee> { temp };
                    IsLoadData = true;
                    SearchInput = string.Empty;
                    ProgressStatus = string.Empty;  
                }
            }
        }
        
        #endregion
    }
}
