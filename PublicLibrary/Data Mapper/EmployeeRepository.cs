// <copyright file="EmployeeRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    /// <summary>
    /// The employee repository.
    /// </summary>
    public class EmployeeRepository
    {
        private readonly LibraryDb libraryDb;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRepository"/> class.
        /// </summary>
        /// <param name="libraryDb">The database connection.</param>
        public EmployeeRepository(LibraryDb libraryDb)
        {
            this.libraryDb = libraryDb;
        }

        /// <summary>
        /// Add a new employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>If employee was created.</returns>
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
    }
}