// <copyright file="EmployeeRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System.Linq;

    /// <summary>
    /// The employee repository.
    /// </summary>
    public class EmployeeRepository
    {
        private readonly LibraryDb libraryDb;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRepository"/> class.
        /// </summary>
        /// <param name="libraryDb">The library database.</param>
        public EmployeeRepository(LibraryDb libraryDb)
        {
            this.libraryDb = libraryDb;
        }

        /// <summary>
        /// Add a new employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>If employee was added.</returns>
        public bool AddEmployee(Employee employee)
        {
            this.libraryDb.Employees.Add(employee);
            var successful = this.libraryDb.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Employee added successfully : {employee.FirstName} {employee.LastName}");
            }
            else
            {
                LoggerUtil.LogError($"Employee failed to add to db : {employee.FirstName} {employee.LastName}");
            }

            return successful;
        }

        /// <summary>
        /// Get employee by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>An employee.</returns>
        public Employee GetEmployee(string email)
        {
            return this.libraryDb.Employees.FirstOrDefault(e => e.Email.Equals(email));
        }
    }
}