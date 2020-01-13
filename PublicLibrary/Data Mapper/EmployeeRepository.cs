using PublicLibrary.Domain_Model;

namespace PublicLibrary.Data_Mapper
{
    public class EmployeeRepository
    {
        private readonly LibraryDb _libraryDb;

        public EmployeeRepository(LibraryDb libraryDb)
        {
            _libraryDb = libraryDb;
        }

        public bool AddEmployee(Employee employee)
        {
            _libraryDb.Employees.Add(employee);
            var successful = _libraryDb.SaveChanges() != 0;
            if (successful) LoggerUtil.LogInfo($"Employee added successfully : {employee.FirstName} {employee.LastName}");
            else LoggerUtil.LogError($"Employee failed to add to db : {employee.FirstName} {employee.LastName}");
            return successful;
        }
    }
}