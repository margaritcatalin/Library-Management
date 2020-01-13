using System.Linq;
using Castle.Core.Internal;
using PublicLibrary.Data_Mapper;

namespace PublicLibrary.BusinessLayer
{
    public class EmployeeService
    {
        private EmployeeRepository _employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public bool AddEmployee(Employee employee)
        {
            if (employee == null) return false;
            if (employee.FirstName.IsNullOrEmpty()) return false;
            if (employee.FirstName.Length < 3 || employee.FirstName.Length > 80) return false;
            if (employee.FirstName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c)))) return false;
            if (char.IsLower(employee.FirstName.First())) return false;

            if (employee.LastName.IsNullOrEmpty()) return false;
            if (employee.LastName.Length < 3 || employee.LastName.Length > 80) return false;
            if (employee.LastName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c)))) return false;
            if (char.IsLower(employee.LastName.First())) return false;


            if (employee.Address.IsNullOrEmpty()) return false;
            if (employee.Address.Length < 3 || employee.Address.Length > 120) return false;
            if (employee.Address.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '.' || c == ','))) return false;

            if (employee.Email.IsNullOrEmpty()) return false;
            if (employee.Email.Length < 10 || employee.Email.Length > 150) return false;
            if (employee.Email.Any(c => !(char.IsLetterOrDigit(c) || c == '@' || c == '.' || c == '_' || c == '-'))) return false;
            if (employee.Email.All(c => c != '@')) return false;

            if (employee.Phone.IsNullOrEmpty()) return false;
            if (employee.Phone.Length != 10) return false;
            if (employee.Phone.Any(c => !(char.IsDigit(c)))) return false;

            return _employeeRepository.AddEmployee(employee);
        }

    }
}