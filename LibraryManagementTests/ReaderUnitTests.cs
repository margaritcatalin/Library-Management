// <copyright file="ReaderUnitTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// Reader unit testes.
    /// </summary>
    [TestFixture]
    public class ReaderUnitTests
    {
        private LibraryDbContext libraryContextMock;

        private ReaderService readerService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            EntityFrameworkMock.PrepareMock(this.libraryContextMock);
            this.readerService = new ReaderService(new ReaderRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add a null reader.
        /// </summary>
        [Test]
        public void TestAddNullReader()
        {
            var result = this.readerService.AddReader(null);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a new reader with empty first Name.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameEmpty()
        {
            var reader = new Reader
                         {
                             FirstName = string.Empty,
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a new reader with null firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameNull()
        {
            var reader = new Reader
                         {
                             FirstName = null,
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test Add a reader with firstName less than 3.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameLessThan3()
        {
            var reader = new Reader
                         {
                             FirstName = "Aa",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with long firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLongFirstName()
        {
            var reader = new Reader
                         {
                             FirstName =
                                 "LongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstNameLongFirstName",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with digit in firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameDigit()
        {
            var reader = new Reader
                         {
                             FirstName = "547",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with symbol in firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameSymbol()
        {
            var reader = new Reader
                         {
                             FirstName = "@#Ion",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with lower case firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameLowerCase()
        {
            var reader = new Reader
                         {
                             FirstName = "horatiu",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with white space in firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameWhiteSpace()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryContextMock.Readers.Count() == 1);
        }

        /// <summary>
        /// Add a reader with lastName Null.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameNull()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Marcus",
                             LastName = null,
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameEmpty()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = string.Empty,
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with lastName less than 3.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameLessThan3()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Ab",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with long lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLongLastName()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName =
                                 "LongLongLOngggggsaasfsssssssssssssssssssssaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaffffffffffsfsssssssssssaaaaaaaaaassssf",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with digit in lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameDigit()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Ay5477",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with symbol in lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameSymbol()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Al@",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with lower case lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameLowerCase()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with space in lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameWhiteSpace()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Joge Virtual",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryContextMock.Readers.Count() == 1);
        }

        /// <summary>
        /// Test add a reader with null address.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullAddress()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = null,
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty address.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmptyAddress()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = string.Empty,
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with address less than 3.
        /// </summary>
        [Test]
        public void TestAddReaderWithAddressLessThan3()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "aa",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with long address.
        /// </summary>
        [Test]
        public void TestAddReaderWithLongAddress()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "LongLongAddressLongLongAddressLongLongAddressLongLongAddressLongLongAddressLongLongAddressLongLongAddressLongLongAddressLongLongAddressLongLongAddress",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with symbol in address.
        /// </summary>
        [Test]
        public void TestAddReaderWithAddressSymbol()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str. Camil Petrescu nr66$@",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with null email.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullEmail()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = null,
                             Phone = null,
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty email.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmptyEmail()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = string.Empty,
                             Phone = null,
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with email less than 10.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmailLessThan10Chars()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "g@t.com",
                             Phone = "0734445567",
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with long email.
        /// </summary>
        [Test]
        public void TestAddReaderWithLongEmail()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "ineedtooolongemailtosave@gooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooogggggggggggggggggggggggggggggggggggglllllllllllleeeeeeeeee.ro",
                             Phone = null,
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with symbol in email.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmailBadSymbol()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "minodor.ionesc###@gmail.com",
                             Phone = null,
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with space in email.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmailWhiteSpace()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "minu ionel@gmail.com",
                             Phone = null,
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with invalid email.
        /// </summary>
        [Test]
        public void TestAddReaderWithBadEmail()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "minorogmail.com",
                             Phone = null,
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with null phone number.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullPhone()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = null,
                             Phone = null,
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty phone.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmptyPhone()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = null,
                             Phone = string.Empty,
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddReaderWithPhoneSmallerLength()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "emaildetest@email.com",
                             Phone = "12345",
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with too big phone number.
        /// </summary>
        [Test]
        public void TestAddReaderWithPhoneBiggerLength()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "emaildetest@email.com",
                             Phone = "123456789123456789",
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddReaderWithPhoneWithLetters()
        {
            var reader = new Reader
                         {
                             FirstName = "Minodor",
                             LastName = "Ionescu",
                             Email = "emaildetest@email.com",
                             Phone = "123456Cargus",
                             Address = "Str. Camil Petrescu nr66",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add reader with null extensions.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullExtensions()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Sandu Ionel",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = null,
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with null gender.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = null,
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty gender.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmptyGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = string.Empty,
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with gender male.
        /// </summary>
        [Test]
        public void TestAddReaderWithMGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryContextMock.Readers.Count() == 1);
        }

        /// <summary>
        /// Test add a reader with gender female.
        /// </summary>
        [Test]
        public void TestAddReaderWithFGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "F",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryContextMock.Readers.Count() == 1);
        }

        /// <summary>
        /// Test add a reader with invalid gender.
        /// </summary>
        [Test]
        public void TestAddReaderWithBadGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Ionescu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "C",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryContextMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test get a reader.
        /// </summary>
        [Test]
        public void TestGetReader()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Sandu Gaga",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            reader = this.readerService.GetReader(reader.Email, reader.Phone);
            Assert.NotNull(reader);
        }

        /// <summary>
        /// Test get a reader by phone.
        /// </summary>
        [Test]
        public void TestGetReaderByPhone()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Sandu Gaga",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            reader = this.readerService.GetReader(null, reader.Phone);
            Assert.NotNull(reader);
        }

        /// <summary>
        /// Test get a reader by email.
        /// </summary>
        [Test]
        public void TestGetReaderByEmail()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Sandu Gaga",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            reader = this.readerService.GetReader(reader.Email, null);
            Assert.NotNull(reader);
        }

        /// <summary>
        /// Test get a reader by null values.
        /// </summary>
        [Test]
        public void TestGetNullReader()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Sandu Gaga",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            reader = this.readerService.GetReader(null, null);
            Assert.Null(reader);
        }

        /// <summary>
        /// Test ge a reader by empty strings.
        /// </summary>
        [Test]
        public void TestGetEmptyReader()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Sandu Gaga",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            reader = this.readerService.GetReader(string.Empty, string.Empty);
            Assert.Null(reader);
        }

        /// <summary>
        /// Test find a unknown reader.
        /// </summary>
        [Test]
        public void TestGetUnknownReader()
        {
            var reader = new Reader
                         {
                             FirstName = "Aly Baba",
                             LastName = "Sandu",
                             Email = "marcu.ionel@gmail.com",
                             Phone = "0765477898",
                             Address = "Str.Camil Petrescu nr.23",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            reader = this.readerService.GetReader("gaga@gmail.com", "0738483459");
            Assert.Null(reader);
        }
    }
}