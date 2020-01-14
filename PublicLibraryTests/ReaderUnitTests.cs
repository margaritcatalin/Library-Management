// <copyright file="ReaderUnitTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibraryTests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using PublicLibrary.BusinessLayer;
    using PublicLibrary.Data_Mapper;
    using PublicLibrary.Domain_Model;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// Reader unit testes.
    /// </summary>
    [TestFixture]
    public class ReaderUnitTests
    {
        private LibraryDb libraryDbMock;

        private ReaderService readerService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryDbMock = EntityFrameworkMock.Create<LibraryDb>();
            EntityFrameworkMock.PrepareMock(this.libraryDbMock);
            this.readerService = new ReaderService(new ReaderRepository(this.libraryDbMock));
        }

        /// <summary>
        /// Test add a null reader.
        /// </summary>
        [Test]
        public void TestAddNullReader()
        {
            var result = this.readerService.AddReader(null);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
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
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
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
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
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
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
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
                                 "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with digit in firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameDigit()
        {
            var reader = new Reader
                         {
                             FirstName = "123",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with symbol in firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameSymbol()
        {
            var reader = new Reader
                         {
                             FirstName = "@#Ana",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with lower case firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameLowerCase()
        {
            var reader = new Reader
                         {
                             FirstName = "ana",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with white space in firstName.
        /// </summary>
        [Test]
        public void TestAddReaderWithFirstNameWhiteSpace()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryDbMock.Readers.Count() == 1);
        }

        /// <summary>
        /// Add a reader with lastName Null.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameNull()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = null,
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameEmpty()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = string.Empty,
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with lastName less than 3.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameLessThan3()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Ak",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with long lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLongLastName()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName =
                                 "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with digit in lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameDigit()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Ak467",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with symbol in lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameSymbol()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Ak@",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with lower case lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameLowerCase()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "alexandrescu",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with space in lastName.
        /// </summary>
        [Test]
        public void TestAddReaderWithLastNameWhiteSpace()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Sad Huseiim",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryDbMock.Readers.Count() == 1);
        }

        /// <summary>
        /// Test add a reader with null address.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullAddress()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = null,
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty address.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmptyAddress()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = string.Empty,
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with address less than 3.
        /// </summary>
        [Test]
        public void TestAddReaderWithAddressLessThan3()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "aa",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with long address.
        /// </summary>
        [Test]
        public void TestAddReaderWithLongAddress()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address =
                                 "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with symbol in address.
        /// </summary>
        [Test]
        public void TestAddReaderWithAddressSymbol()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str. Memorandului nr4$@",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with null email.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullEmail()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = null,
                             Phone = null,
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty email.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmptyEmail()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = string.Empty,
                             Phone = null,
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with email less than 10.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmailLessThan10Chars()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = "@yahoo.co",
                             Phone = null,
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with long email.
        /// </summary>
        [Test]
        public void TestAddReaderWithLongEmail()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email =
                                 "@aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                             Phone = null,
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with symbol in email.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmailBadSymbol()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = "alexandru.v###@gmail.com",
                             Phone = null,
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with space in email.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmailWhiteSpace()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = "alexandru v@gmail.com",
                             Phone = null,
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with invalid email.
        /// </summary>
        [Test]
        public void TestAddReaderWithBadEmail()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = "alexandrugmail.com",
                             Phone = null,
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with null phone number.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullPhone()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = null,
                             Phone = null,
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty phone.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmptyPhone()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = null,
                             Phone = string.Empty,
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddReaderWithPhoneSmallerLength()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = null,
                             Phone = "12345",
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with too big phone number.
        /// </summary>
        [Test]
        public void TestAddReaderWithPhoneBiggerLength()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = null,
                             Phone = "123456789123456789",
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with invalid phone number.
        /// </summary>
        [Test]
        public void TestAddReaderWithPhoneWithLetters()
        {
            var reader = new Reader
                         {
                             FirstName = "Valentin",
                             LastName = "Alexandru",
                             Email = null,
                             Phone = "123456Vali",
                             Address = "Str. Memorandului nr4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };

            var result = this.readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add reader with null extensions.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullExtensions()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Sad Huseiim",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = null,
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with null gender.
        /// </summary>
        [Test]
        public void TestAddReaderWithNullGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = null,
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with empty gender.
        /// </summary>
        [Test]
        public void TestAddReaderWithEmptyGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = string.Empty,
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test add a reader with gender male.
        /// </summary>
        [Test]
        public void TestAddReaderWithMGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryDbMock.Readers.Count() == 1);
        }

        /// <summary>
        /// Test add a reader with gender female.
        /// </summary>
        [Test]
        public void TestAddReaderWithFGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "F",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryDbMock.Readers.Count() == 1);
        }

        /// <summary>
        /// Test add a reader with invalid gender.
        /// </summary>
        [Test]
        public void TestAddReaderWithBadGender()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Alexandru",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "C",
                         };
            var result = this.readerService.AddReader(reader);
            Assert.True(this.libraryDbMock.Readers.Count() == 0);
        }

        /// <summary>
        /// Test get a reader.
        /// </summary>
        [Test]
        public void TestGetReader()
        {
            var reader = new Reader
                         {
                             FirstName = "Al Alekku",
                             LastName = "Sad Huseiim",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
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
                             FirstName = "Al Alekku",
                             LastName = "Sad Huseiim",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
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
                             FirstName = "Al Alekku",
                             LastName = "Sad Huseiim",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
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
                             FirstName = "Al Alekku",
                             LastName = "Sad Huseiim",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
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
                             FirstName = "Al Alekku",
                             LastName = "Sad Huseiim",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
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
                             FirstName = "Al Alekku",
                             LastName = "Sad Huseiim",
                             Email = "alexandru.v@yahoo.com",
                             Phone = "7345345568",
                             Address = "Str.Memorandului nr.4",
                             Extensions = new List<Extension>(),
                             Gender = "M",
                         };
            var result = this.readerService.AddReader(reader);
            reader = this.readerService.GetReader("NicuRata@gmail.com", "0738489489");
            Assert.Null(reader);
        }
    }
}