// <copyright file="EmployeeService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.BusinessLayer
{
    using System.Linq;
    using Castle.Core.Internal;
    using PublicLibrary.Data_Mapper;

    /// <summary>
    /// The employee service.
    /// </summary>
    public class EmployeeService
    {
        private EmployeeRepository employeeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="employeeRepository">The employee repository.</param>
        public EmployeeService(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Create a new employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>If employee was created.</returns>
        public bool AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                LoggerUtil.LogInfo($"The employee is null.");
                return false;
            }

            if (employee.FirstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The employee name is null or emply.");
                return false;
            }

            if (employee.FirstName.Length < 3 || employee.FirstName.Length > 80)
            {
                LoggerUtil.LogInfo($"The employee name is invalid.");
                return false;
            }

            if (employee.FirstName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"The employee name is invalid.");
                return false;
            }

            if (char.IsLower(employee.FirstName.First()))
            {
                LoggerUtil.LogInfo($"The employee first name is started with lower case.");
                return false;
            }

            if (employee.LastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The employee last name is null or empty.");
                return false;
            }

            if (employee.LastName.Length < 3 || employee.LastName.Length > 80)
            {
                LoggerUtil.LogInfo($"The employee last name is invalid.");
                return false;
            }

            if (employee.LastName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"The employee last name is invalid.");
                return false;
            }

            if (char.IsLower(employee.LastName.First()))
            {
                LoggerUtil.LogInfo($"The employee last name is started with lower case.");
                return false;
            }

            if (employee.Address.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The employee address is null or empty.");
                return false;
            }

            if (employee.Address.Length < 3 || employee.Address.Length > 120)
            {
                LoggerUtil.LogInfo($"The employee address is invalid.");
                return false;
            }

            if (employee.Address.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '.' || c == ',')))
            {
                LoggerUtil.LogInfo($"The employee address is invalid.");
                return false;
            }

            if (employee.Email.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The employee email is null or empty.");
                return false;
            }

            if (employee.Email.Length < 10 || employee.Email.Length > 150)
            {
                LoggerUtil.LogInfo($"The employee email is invalid.");
                return false;
            }

            if (employee.Email.Any(c => !(char.IsLetterOrDigit(c) || c == '@' || c == '.' || c == '_' || c == '-')))
            {
                LoggerUtil.LogInfo($"The employee email is invalid.");
                return false;
            }

            if (employee.Email.All(c => c != '@'))
            {
                LoggerUtil.LogInfo($"The employee address is invalid.");
                return false;
            }

            if (employee.Phone.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The employee phone number is null or empty.");
                return false;
            }

            if (employee.Phone.Length != 10)
            {
                LoggerUtil.LogInfo($"The employee phone number is invalid.");
                return false;
            }

            if (employee.Phone.Any(c => !char.IsDigit(c)))
            {
                LoggerUtil.LogInfo($"The employee phone number is invalid.");
                return false;
            }

            return this.employeeRepository.AddEmployee(employee);
        }
    }
}