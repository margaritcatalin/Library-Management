// <copyright file="EmployeeUnitTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibraryTests
{
    using System.Linq;
    using NUnit.Framework;
    using PublicLibrary.BusinessLayer;
    using PublicLibrary.Data_Mapper;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// Employee unit tests.
    /// </summary>
    [TestFixture]
    public class EmployeeUnitTests
    {
        private LibraryDb libraryDbMock;

        private EmployeeService employeeService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryDbMock = EntityFrameworkMock.Create<LibraryDb>();
            EntityFrameworkMock.PrepareMock(this.libraryDbMock);
            this.employeeService = new EmployeeService(new EmployeeRepository(this.libraryDbMock));
        }

        /// <summary>
        /// Test add a null employee.
        /// </summary>
        [Test]
        public void TestAddNullEmployee()
        {
            var result = this.employeeService.AddEmployee(null);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with null firstName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithNullFirstName()
        {
            var employee = new Employee()
                           {
                               FirstName = null,
                               LastName = "alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with empty first name.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithEmptyFirstName()
        {
            var employee = new Employee()
                           {
                               FirstName = string.Empty,
                               LastName = "alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with first name less than 3.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithFirstNameLessThan3()
        {
            var employee = new Employee()
                           {
                               FirstName = "aa",
                               LastName = "alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with long firstName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithLongFirstName()
        {
            var employee = new Employee()
                           {
                               FirstName =
                                   "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                               LastName = "alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with digit in firstName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithFirstNameDigit()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali5",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with symbol in firstName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithFirstNameSymbol()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali@@##$@#",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with lower case for firstName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithFirstNameLowerCase()
        {
            var employee = new Employee()
                           {
                               FirstName = "vali",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with white space in firstName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithFirstNameWhiteSpace()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali Al Alekku",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.True(this.libraryDbMock.Employees.Count() == 1);
        }

        /// <summary>
        /// Test add employee with null lastName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithNullLastName()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName = null,
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with empty lastName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithEmptyLastName()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName = string.Empty,
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with invalid last Name.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithLastNameLessThan3()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName = "Al",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with long lastName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithLongLastName()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName =
                                   "LongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLast",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with symbol in lastName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithLastNameSymbol()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName = "Alexandru@@#@$%",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with digit in lastName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithLastNameDigit()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName = "Alexandru12",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with lastName with lower case.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithLastNameLowerCase()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName = "alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with white space in lastName.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithLastNameWhiteSpace()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName = "Alexandru Al Alehu",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.True(this.libraryDbMock.Employees.Count() == 1);
        }

        /// <summary>
        /// Test add employee with null address.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithNullAddress()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = null,
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with empty address.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithEmptyAddress()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = string.Empty,
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with address less than 3.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithAddressLessThan3()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "aa",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with long address.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithLongAddress()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address =
                                   "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with symbol in address.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithAddressSymbol()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str. Memorandului nr4$%",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with email null.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithNullEmail()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = null,
                               Phone = "7345345568",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with empty email.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithEmptyEmail()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = string.Empty,
                               Phone = "7345345568",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with email less than 10.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithEmailLessThan10()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "@yahoo.co",
                               Phone = "7345345568",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with long email.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithLongEmail()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email =
                                   "@LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong.ro",
                               Phone = "7345345568",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with symbol in email.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithEmailSymbols()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru.v$#%%#@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with space in email.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithBadEmailWhiteSpace()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with invalid email.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithBadEmailNoSymbol()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandruyahoo.com",
                               Phone = "7345345568",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with null phone.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithNullPhone()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = null,
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with empty phone number.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithEmptyPhone()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = string.Empty,
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithPhoneSmallerLength()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = "12345",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithPhoneBiggerLength()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = "123456789123456789",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithPhoneWithLetters()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = "123456Vali",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with null gender.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithNullGender()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = "1234567890",
                               Address = "Str. Memorandului nr4",
                               Gender = null,
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with empty gender.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithEmptyGender()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = "1234567890",
                               Address = "Str. Memorandului nr4",
                               Gender = string.Empty,
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test add employee with gender male.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithMGender()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = "1234567890",
                               Address = "Str. Memorandului nr4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.True(this.libraryDbMock.Employees.Count() == 1);
        }

        /// <summary>
        /// Test add employee with gender female.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithFGender()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = "1234567890",
                               Address = "Str. Memorandului nr4",
                               Gender = "F",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.True(this.libraryDbMock.Employees.Count() == 1);
        }

        /// <summary>
        /// Test add employee with unknown gender.
        /// </summary>
        [Test]
        public void TestAddEmployeeWithBadGender()
        {
            var employee = new Employee
                           {
                               FirstName = "Valentin",
                               LastName = "Alexandru",
                               Email = "alexandru@yahoo.com",
                               Phone = "1234567890",
                               Address = "Str. Memorandului nr4",
                               Gender = "C",
                           };

            var result = this.employeeService.AddEmployee(employee);
            Assert.True(this.libraryDbMock.Employees.Count() == 0);
        }

        /// <summary>
        /// Test get an employee.
        /// </summary>
        [Test]
        public void TestGetGoodEmployee()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName = "Alexandru Al Alehu",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            employee = this.employeeService.GetEmployee(employee.Email);
            Assert.True(employee != null);
        }

        /// <summary>
        /// Test get an employee by null email.
        /// </summary>
        [Test]
        public void TestGetNullEmployee()
        {
            var employee = this.employeeService.GetEmployee(null);
            Assert.False(employee != null);
        }

        /// <summary>
        /// Testget an employee with empty email.
        /// </summary>
        [Test]
        public void TestGetEmptyEmployee()
        {
            var employee = this.employeeService.GetEmployee(string.Empty);
            Assert.False(employee != null);
        }

        /// <summary>
        /// Test gen an unknown employee.
        /// </summary>
        [Test]
        public void TestGetUnknownEmployee()
        {
            var employee = new Employee()
                           {
                               FirstName = "Vali",
                               LastName = "Alexandru Al Alehu",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "7345345568",
                               Address = "Str.Memorandului nr.4",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            employee = this.employeeService.GetEmployee("rata@gmail.com");
            Assert.False(employee != null);
        }
    }
}