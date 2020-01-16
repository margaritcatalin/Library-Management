// <copyright file="LibrarianUnitTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System.Linq;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// Librarian unit tests.
    /// </summary>
    [TestFixture]
    public class LibrarianUnitTests
    {
        private LibraryDbContext libraryContextMock;

        private LibrarianService librarianService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            EntityFrameworkMock.PrepareMock(this.libraryContextMock);
            this.librarianService = new LibrarianService(new LibrarianRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add a null librarian.
        /// </summary>
        [Test]
        public void TestAddNullLibrarian()
        {
            var result = this.librarianService.AddLibrarian(null);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with null firstName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithNullFirstName()
        {
            var librarian = new Librarian()
                           {
                               FirstName = null,
                               LastName = "Valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with empty first name.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithEmptyFirstName()
        {
            var librarian = new Librarian()
                           {
                               FirstName = string.Empty,
                               LastName = "Valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with first name less than 3.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithFirstNameLessThan3()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "aa",
                               LastName = "Valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with long firstName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithLongFirstName()
        {
            var librarian = new Librarian()
                           {
                               FirstName =
                                   "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                               LastName = "Valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with digit in firstName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithFirstNameDigit()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marcu78",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with symbol in firstName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithFirstNameSymbol()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian@@##$@#",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with lower case for firstName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithFirstNameLowerCase()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "marcu",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with white space in firstName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithFirstNameWhiteSpace()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian Al Alekku",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.True(this.libraryContextMock.Librarians.Count() == 1);
        }

        /// <summary>
        /// Test add librarian with null lastName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithNullLastName()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName = null,
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with empty lastName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithEmptyLastName()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName = string.Empty,
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with invalid last Name.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithLastNameLessThan3()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName = "Al",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with long lastName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithLongLastName()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName =
                                   "LongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLastLongLast",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with symbol in lastName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithLastNameSymbol()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName = "Alexandru@@#@$%",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with digit in lastName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithLastNameDigit()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName = "Alexandru12",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with lastName with lower case.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithLastNameLowerCase()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName = "valeriu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with white space in lastName.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithLastNameWhiteSpace()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName = "Alexandru Al Alehu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.True(this.libraryContextMock.Librarians.Count() == 1);
        }

        /// <summary>
        /// Test add librarian with null address.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithNullAddress()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = null,
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with empty address.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithEmptyAddress()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = string.Empty,
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with address less than 3.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithAddressLessThan3()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "aa",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with long address.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithLongAddress()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address =
                                   "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with symbol in address.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithAddressSymbol()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66$%",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with email null.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithNullEmail()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = null,
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with empty email.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithEmptyEmail()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = string.Empty,
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with email less than 10.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithEmailLessThan10()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "t@t.com",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with long email.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithLongEmail()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email =
                                   "@LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong.ro",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with symbol in email.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithEmailSymbols()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "ionescu.minu$#%%#@mailinator.com",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with space in email.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithBadEmailWhiteSpace()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "ionescu minu@google.com",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with invalid email.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithBadEmailNoSymbol()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "invalidemailgmail.com",
                               Phone = "0765477898",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with null phone.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithNullPhone()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = null,
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with empty phone number.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithEmptyPhone()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = string.Empty,
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithPhoneSmallerLength()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "12345",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithPhoneBiggerLength()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "123456789123456789",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithPhoneWithLetters()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "123456Vali",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with null gender.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithNullGender()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = null,
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with empty gender.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithEmptyGender()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = string.Empty,
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test add librarian with gender male.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithMGender()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.True(this.libraryContextMock.Librarians.Count() == 1);
        }

        /// <summary>
        /// Test add librarian with gender female.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithFGender()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "F",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.True(this.libraryContextMock.Librarians.Count() == 1);
        }

        /// <summary>
        /// Test add librarian with unknown gender.
        /// </summary>
        [Test]
        public void TestAddLibrarianWithBadGender()
        {
            var librarian = new Librarian
                           {
                               FirstName = "Minodor",
                               LastName = "Ionescu",
                               Email = "manolescu@gmail.com",
                               Phone = "1234567890",
                               Address = "Str. Camil Petrescu nr66",
                               Gender = "C",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            Assert.True(this.libraryContextMock.Librarians.Count() == 0);
        }

        /// <summary>
        /// Test get an librarian.
        /// </summary>
        [Test]
        public void TestGetGoodLibrarian()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName = "Marcus Al Alehu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            librarian = this.librarianService.GetLibrarian(librarian.Email);
            Assert.True(librarian != null);
        }

        /// <summary>
        /// Test get an librarian by null email.
        /// </summary>
        [Test]
        public void TestGetNullLibrarian()
        {
            var librarian = this.librarianService.GetLibrarian(null);
            Assert.False(librarian != null);
        }

        /// <summary>
        /// Testget an librarian with empty email.
        /// </summary>
        [Test]
        public void TestGetEmptyLibrarian()
        {
            var librarian = this.librarianService.GetLibrarian(string.Empty);
            Assert.False(librarian != null);
        }

        /// <summary>
        /// Test gen an unknown librarian.
        /// </summary>
        [Test]
        public void TestGetUnknownLibrarian()
        {
            var librarian = new Librarian()
                           {
                               FirstName = "Marian",
                               LastName = "Marcus Al Alehu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "0765477898",
                               Address = "Str.Camil Petrescu nr.23",
                               Gender = "M",
                           };

            var result = this.librarianService.AddLibrarian(librarian);
            librarian = this.librarianService.GetLibrarian("glorios@mailinator.com");
            Assert.False(librarian != null);
        }
    }
}