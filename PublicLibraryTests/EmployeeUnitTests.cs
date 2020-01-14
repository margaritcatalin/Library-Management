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
                               LastName = "Valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               LastName = "Valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               LastName = "Valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               LastName = "Valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marcu78",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian@@##$@#",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "marcu",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian Al Alekku",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian",
                               LastName = null,
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian",
                               LastName = string.Empty,
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian",
                               LastName = "Al",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian",
                               LastName =
                                   "LongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLast",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian",
                               LastName = "Alexandru@@#@$%",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian",
                               LastName = "Alexandru12",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian",
                               LastName = "Valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian",
                               LastName = "Alexandru Al Alehu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66$%",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = null,
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = string.Empty,
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "@google.co",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email =
                                   "@LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong.ro",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "ionescu.minu$#%%#@mailinator.com",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "ionescu minu@google.com",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "invalidemailgmail.com",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = null,
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = string.Empty,
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "12345",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "123456789123456789",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "123456Vali",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
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
                               FirstName = "Marian",
                               LastName = "Marcus Al Alehu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
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
                               FirstName = "Marian",
                               LastName = "Marcus Al Alehu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.employeeService.AddEmployee(employee);
            employee = this.employeeService.GetEmployee("glorios@mailinator.com");
            Assert.False(employee != null);
        }
    }
}