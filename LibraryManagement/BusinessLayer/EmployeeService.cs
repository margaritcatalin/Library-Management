// <copyright file="EmployeeService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.BusinessLayer
{
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;

    /// <summary>
    /// The employee service.
    /// </summary>
    public class EmployeeService
    {
        private readonly EmployeeRepository employeeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="employeeRepository">The employee repository.</param>
        public EmployeeService(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Add a new employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>If employee was created.</returns>
        public bool AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee is null.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.FirstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee firstName is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((employee.FirstName.Length < 3) || (employee.FirstName.Length > 80))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee firstName has a invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.FirstName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee firstName is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(employee.FirstName.First()))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee firstName is need to start with uppercase.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.LastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee lastName is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((employee.LastName.Length < 3) || (employee.LastName.Length > 80))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee lastName length is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.LastName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee lastName is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(employee.LastName.First()))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee lastName is need to start with upperCase.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.Address.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee address is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((employee.Address.Length < 3) || (employee.Address.Length > 120))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee address has an invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.Address.Any(
                c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || (c == '.') || (c == ','))))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee address is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.Email.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee email is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((employee.Email.Length < 10) || (employee.Email.Length > 150))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee email has invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.Email.Any(
                c => !(char.IsLetterOrDigit(c) || (c == '@') || (c == '.') || (c == '_') || (c == '-'))))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee email is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.Email.All(c => c != '@'))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee email is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.Phone.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee phone is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.Phone.Length != 10)
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee phone has invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.Phone.Any(c => !char.IsDigit(c)))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee phone is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (employee.Gender.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee gerner is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!(employee.Gender.Equals("M") || employee.Gender.Equals("F")))
            {
                LoggerUtil.LogInfo($"Your employee is invalid. Employee gender is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            return this.employeeRepository.AddEmployee(employee);
        }

        /// <summary>
        /// Get employee by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>An employee.</returns>
        public Employee GetEmployee(string email)
        {
            if (email.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Param email is required.", MethodBase.GetCurrentMethod());
                return null;
            }

            return this.employeeRepository.GetEmployee(email);
        }
    }
}