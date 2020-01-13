using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PublicLibrary.BusinessLayer;
using PublicLibrary.Data_Mapper;
using PublicLibrary.Domain_Model;
using Telerik.JustMock.EntityFramework;

namespace PublicLibraryTests
{
    [TestFixture]
    public class ReaderUnitTests
    {
        private LibraryDb _libraryDbMock;
        private ReaderService _readerService;

        [SetUp]
        public void SetUp()
        {
            _libraryDbMock = EntityFrameworkMock.Create<LibraryDb>();
            EntityFrameworkMock.PrepareMock(_libraryDbMock);
            _readerService = new ReaderService(new ReaderRepository(_libraryDbMock));
        }

        [Test]
        public void TestAddNullReader()
        {
            var result = _readerService.AddReader(null);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithFirstNameEmpty()
        {
            Reader reader = new Reader
            {
                FirstName = "",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithFirstNameNull()
        {
            Reader reader = new Reader
            {
                FirstName = null,
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithFirstNameLessThan3()
        {
            Reader reader = new Reader
            {
                FirstName = "Aa",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithLongFirstName()
        {
            Reader reader = new Reader
            {
                FirstName =
                    "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithFirstNameDigit()
        {
            Reader reader = new Reader
            {
                FirstName = "123",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithFirstNameSymbol()
        {
            Reader reader = new Reader
            {
                FirstName = "@#Ana",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }
        [Test]
        public void TestAddReaderWithFirstNameLowerCase()
        {
            Reader reader = new Reader
            {
                FirstName = "ana",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithFirstNameWhiteSpace()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);
            Assert.True(_libraryDbMock.Readers.Count() == 1);
        }

        [Test]
        public void TestAddReaderWithLastNameNull()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = null,
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithLastNameEmpty()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithLastNameLessThan3()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "Ak",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithLongLastName()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName =
                    "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithLastNameDigit()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "Ak467",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithLastNameSymbol()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "Ak@",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }
        [Test]
        public void TestAddReaderWithLastNameLowerCase()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "alexandrescu",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithLastNameWhiteSpace()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "Sad Huseiim",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);
            Assert.True(_libraryDbMock.Readers.Count() == 1);
        }

        [Test]
        public void TestAddReaderWithNullAddress()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = null,
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithEmptyAddress()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithAddressLessThan3()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "aa",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithLongAddress()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address =
                    "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithAddressSymbol()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str. Memorandului nr4$@",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithNullEmail()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = null,
                Phone = null,
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithEmptyEmail()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "",
                Phone = null,
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithEmailLessThan10Chars()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "@yahoo.co",
                Phone = null,
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithLongEmail()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email =
                    "@aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                Phone = null,
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithEmailBadSymbol()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru.v###@gmail.com",
                Phone = null,
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithEmailWhiteSpace()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandru v@gmail.com",
                Phone = null,
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithBadEmail()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = "alexandrugmail.com",
                Phone = null,
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithNullPhone()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = null,
                Phone = null,
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithEmptyPhone()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = null,
                Phone = "",
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithPhoneSmallerLength()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = null,
                Phone = "12345",
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithPhoneBiggerLength()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = null,
                Phone = "123456789123456789",
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }

        [Test]
        public void TestAddReaderWithPhoneWithLetters()
        {
            Reader reader = new Reader
            {
                FirstName = "Valentin",
                LastName = "Alexandru",
                Email = null,
                Phone = "123456Vali",
                Address = "Str. Memorandului nr4",
                Extensions = new List<Extension>()
            };

            var result = _readerService.AddReader(reader);
            Assert.False(result);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }
        [Test]
        public void TestAddReaderWithNullExtensions()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "Sad Huseiim",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = null
            };
            var result = _readerService.AddReader(reader);
            Assert.True(_libraryDbMock.Readers.Count() == 0);
        }
        /*[Test]
        public void AddExtension()
        {
            Reader reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "7345345568",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
            var result = _readerService.AddReader(reader);

            _readerService.AddExtension(reader);
            Assert.True(reader.Extensions.Count() == 1);
        }
        [Test]
        public void AddExtensionNullReader()
        {
            var result =_readerService.AddExtension(null);
            Assert.False(result);
        }*/
    }
}