using System;
using System.Linq;
using NUnit.Framework;
using PublicLibrary.BusinessLayer;
using PublicLibrary.Data_Mapper;
using PublicLibrary.Domain_Model;
using Telerik.JustMock.EntityFramework;

namespace PublicLibraryTests
{
    [TestFixture]
    public class EmployeeUnitTests
    {
        private LibraryDb _libraryDbMock;
        private EmployeeService _employeeService;

        [SetUp]
        public void SetUp()
        {
            _libraryDbMock = EntityFrameworkMock.Create<LibraryDb>();
            EntityFrameworkMock.PrepareMock(_libraryDbMock);
            _employeeService = new EmployeeService(new EmployeeRepository(_libraryDbMock));
        }

        [Test]
        public void TestAddNullEmployee()
        {
            var result = _employeeService.AddEmployee(null);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithNullFirstName()
        {
            Employee employee = new Employee()
            {
                FirstName = null,
                LastName = "alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithEmptyFirstName()
        {
            Employee employee = new Employee()
            {
                FirstName = "",
                LastName = "alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithFirstNameLessThan3()
        {
            Employee employee = new Employee()
            {
                FirstName = "aa",
                LastName = "alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithLongFirstName()
        {
            Employee employee = new Employee()
            {
                FirstName = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                LastName = "alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithFirstNameDigit()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali5",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithFirstNameSymbol()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali@@##$@#",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithFirstNameLowerCase()
        {
            Employee employee = new Employee()
            {
                FirstName = "vali",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithFirstNameWhiteSpace()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali Al Alekku",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.True(_libraryDbMock.Employees.Count() == 1);
        }
        [Test]
        public void TestAddEmployeeWithNullLastName()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali",
                LastName = null,
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithEmptyLastName()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali",
                LastName = "",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithLastNameLessThan3()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali",
                LastName = "Al",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithLongLastName()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali",
                LastName = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithLastNameSymbol()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali",
                LastName = "Alexandru@@#@$%",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithLastNameDigit()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali",
                LastName = "Alexandru12",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithLastNameLowerCase()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali",
                LastName = "alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithLastNameWhiteSpace()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali",
                LastName = "Alexandru Al Alehu",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.True(_libraryDbMock.Employees.Count() == 1);
        }

        [Test]
        public void TestAddEmployeeWithNullAddress()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = null,
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithEmptyAddress()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithAddressLessThan3()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "aa",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithLongAddress()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address =
                    "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithAddressSymbol()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str. Memorandului nr4$%",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithNullEmail()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = null,
                Phone = "7345345568",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithEmptyEmail()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "",
                Phone = "7345345568",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithEmailLessThan10()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "@yahoo.co",
                Phone = "7345345568",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithLongEmail()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "@AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                Phone = "7345345568",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithEmailSymbols()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v$#%%#@yahoo.com",
                Phone = "7345345568",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithBadEmailWhiteSpace()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru v@yahoo.com",
                Phone = "7345345568",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }
        [Test]
        public void TestAddEmployeeWithBadEmailNoSymbol()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandruyahoo.com",
                Phone = "7345345568",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithNullPhone()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru@yahoo.com",
                Phone = null,
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithEmptyPhone()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru@yahoo.com",
                Phone = "",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithPhoneSmallerLength()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru@yahoo.com",
                Phone = "12345",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithPhoneBiggerLength()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru@yahoo.com",
                Phone = "123456789123456789",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        [Test]
        public void TestAddEmployeeWithPhoneWithLetters()
        {
            Employee employee = new Employee
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru@yahoo.com",
                Phone = "123456Vali",
                Address = "Str. Memorandului nr4",
            };

            var result = _employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(_libraryDbMock.Employees.Count() == 0);
        }

        public void TestGetEmployee()
        {
            Employee employee = new Employee()
            {
                FirstName = "Vali",
                LastName = "Alexandru Al Alehu",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
            };

            var result = _employeeService.AddEmployee(employee);
            
        }
        //get good employee
        //get inexisting employee
    }
}
