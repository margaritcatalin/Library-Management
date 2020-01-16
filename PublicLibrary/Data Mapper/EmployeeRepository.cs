// <copyright file="EmployeeRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.Data_Mapper
{
    using System.Linq;

    /// <summary>
    /// The employee repository.
    /// </summary>
    public class EmployeeRepository
    {
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The library database.</param>
        public EmployeeRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>If employee was added.</returns>
        public bool AddEmployee(Employee employee)
        {
            this.libraryContext.Employees.Add(employee);
            var successful = this.libraryContext.SaveChanges() != 0;
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
            return this.libraryContext.Employees.FirstOrDefault(e => e.Email.Equals(email));
        }
    }
}