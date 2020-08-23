// <copyright file="AuctionUserTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System.Configuration;
using LibraryManagement.DomainModel;
using LibraryManagement.Util;

namespace LibraryManagementTests
{
    using System.Linq;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The auctionUser unit tests.
    /// </summary>
    [TestFixture]
    public class AuctionUserTests
    {
        private LibraryDbContext libraryContextMock;

        private AuctionUserService auctionUserService;
        private UserReviewService userReviewService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            this.userReviewService = new UserReviewService(new UserReviewRepository(this.libraryContextMock));
            this.auctionUserService = new AuctionUserService(new AuctionUserRepository(this.libraryContextMock),
                this.userReviewService);
        }

        /// <summary>
        /// Test add a new auctionUser.
        /// </summary>
        [Test]
        public void TestAddAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(this.libraryContextMock.AuctionUsers.Count() == 1);
        }

        /// <summary>
        /// Test add a null auctionUser.
        /// </summary>
        [Test]
        public void TestAddNullAuctionUser()
        {
            var result = this.auctionUserService.AddAuctionUser(null, Role.Seller);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add an auctionUser with null role.
        /// </summary>
        [Test]
        public void TestAddAuctionUserWithNullRole()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, null);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }


        /// <summary>
        /// Test add an auctionUser with null firstname.
        /// </summary>
        [Test]
        public void TestAddNullFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = null, LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add add auctionUser with empty firstName.
        /// </summary>
        [Test]
        public void TestAddEmptyFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = string.Empty, LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with smaller firstName.
        /// </summary>
        [Test]
        public void TestAddSmallerFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Aa", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with longer firstName.
        /// </summary>
        [Test]
        public void TestAddLongerFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser
            {
                FirstName =
                    "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                LastName = "Pascu",
                Gender = "M",
            };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with lower firstname.
        /// </summary>
        [Test]
        public void TestAddLowerFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Add auctionUser with digit in firstName.
        /// </summary>
        [Test]
        public void TestAddDigitFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Al13Mihai", LastName = "Cioran", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add an auctionUser with firstname with symbols.
        /// </summary>
        [Test]
        public void TestAddSymbolFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Aly$^@*#^*", LastName = "Baba", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(this.libraryContextMock.AuctionUsers.Count() == 0);
        }

        /// <summary>
        /// Test add auctionUser with spaces in firstName.
        /// </summary>
        [Test]
        public void TestAddWhiteSpaceFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Aly Baba", LastName = "Ionel", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(this.libraryContextMock.AuctionUsers.Count() == 1);
        }

        /// <summary>
        /// Test add auctionUser with dash in first name.
        /// </summary>
        [Test]
        public void TestAddDashFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Aly-Baba", LastName = "Kalu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(this.libraryContextMock.AuctionUsers.Count() == 1);
        }

        /// <summary>
        /// Test add auctionUser with null last name.
        /// </summary>
        [Test]
        public void TestAddNullLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Horatiu", LastName = null, Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with empty last name.
        /// </summary>
        [Test]
        public void TestAddEmptyLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Horatiu", LastName = string.Empty, Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test added auctionUser with smaller lastName.
        /// </summary>
        [Test]
        public void TestAddSmallerLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Horatiu", LastName = "small", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with longer lastname.
        /// </summary>
        [Test]
        public void TestAddLongerLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser
            {
                FirstName = "Horatiu",
                LastName =
                    "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                Gender = "M",
            };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with digit lastName.
        /// </summary>
        [Test]
        public void TestAddDigitLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Fratele", LastName = "Galusca5", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with symbol in lastname.
        /// </summary>
        [Test]
        public void TestAddSymbolLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Horatiu", LastName = "Vlad$@%^@#%$)_*", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with lowercase lastname.
        /// </summary>
        [Test]
        public void TestAddLowercaseLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Case", LastName = "lower", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Add auctionUser with space in lastName.
        /// </summary>
        [Test]
        public void TestAddWhiteSpaceLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Estera", LastName = "Aly Baba", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(this.libraryContextMock.AuctionUsers.Count() == 1);
        }

        /// <summary>
        /// Test add auctionUser with dash in lastName.
        /// </summary>
        [Test]
        public void TestAddDashLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Galusca", LastName = "Aly-Baba", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(this.libraryContextMock.AuctionUsers.Count() == 1);
        }

        /// <summary>
        /// Test add auctionUser with null gender.
        /// </summary>
        [Test]
        public void TestAddNullGenderAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = null};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with empty gender.
        /// </summary>
        [Test]
        public void TestAddEmptyGenderAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = string.Empty};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test add auctionUser with gender Male.
        /// </summary>
        [Test]
        public void TestAddMGenderAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(this.libraryContextMock.AuctionUsers.Count() == 1);
        }

        /// <summary>
        /// Test add auctionUser with gender Female.
        /// </summary>
        [Test]
        public void TestAddFGenderAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "F"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(this.libraryContextMock.AuctionUsers.Count() == 1);
        }

        /// <summary>
        /// Test add auctionUser with invalid gender.
        /// </summary>
        [Test]
        public void TestAddBadGenderAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "C"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test get auctionUser.
        /// </summary>
        [Test]
        public void TestGetAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser =
                this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                    auctionUser.LastName);
            Assert.NotNull(auctionUser);
        }

        /// <summary>
        /// Test get auctionUser with null firstname.
        /// </summary>
        [Test]
        public void TestGetNullFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName(null, auctionUser.LastName);
            Assert.Null(auctionUser);
        }

        /// <summary>
        /// Test get auctionUser with null lastName.
        /// </summary>
        [Test]
        public void TestGetNullLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Estera", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, null);
            Assert.Null(auctionUser);
        }

        /// <summary>
        /// Test get AuctionUser with empty firstName.
        /// </summary>
        [Test]
        public void TestGetEmptyFirstNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser =
                this.auctionUserService.GetAuctionUserByFistNameAndLastName(string.Empty, auctionUser.LastName);
            Assert.Null(auctionUser);
        }

        /// <summary>
        /// Test get auctionUser with empty lastName.
        /// </summary>
        [Test]
        public void TestGetEmptyLastNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser =
                this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, string.Empty);
            Assert.Null(auctionUser);
        }

        /// <summary>
        /// Test get auctionUser and he is not in db.
        /// </summary>
        [Test]
        public void TestGetBadNameAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Teodorescu");
            Assert.Null(auctionUser);
        }

        /// <summary>
        /// Test get auctionUser by id and he is in the db.
        /// </summary>
        [Test]
        public void TestGetAuctionUserByGoodId()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var auctionUserById = this.auctionUserService.GetAuctionUserById(auctionUser.Id);
            Assert.NotNull(auctionUserById);
        }

        /// <summary>
        /// Test get auctionUser by id and he is not in the db.
        /// </summary>
        [Test]
        public void TestGetAuctionUserByBadId()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var auctionUserById = this.auctionUserService.GetAuctionUserById(auctionUser.Id + 1);
            Assert.Null(auctionUserById);
        }

        /// <summary>
        /// Test get all auctionUsers.
        /// </summary>
        [Test]
        public void TestGetAllAuctionUsers()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var auctionUser2 = new AuctionUser {FirstName = "Popescu", LastName = "Andrei", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Seller);

            var users = this.auctionUserService.GetAuctionUsers();
            Assert.IsTrue(users.Count() == 2);
        }

        /// <summary>
        /// Test get all auctionUsers with a wrong user.
        /// </summary>
        [Test]
        public void TestGetAllAuctionUsersWithAWrongUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var auctionUser2 = new AuctionUser {FirstName = null, LastName = "Andrei", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Seller);

            var users = this.auctionUserService.GetAuctionUsers();
            Assert.IsFalse(users.Count() == 2);
        }

        /// <summary>
        /// Test update firstName for auction user.
        /// </summary>
        [Test]
        public void TestUpdateFirstNameForAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.FirstName = "Marcus";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userUpdated = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Marcus", "Pascu");

            Assert.IsTrue(user == null && userUpdated != null);
        }

        /// <summary>
        /// Test update with a firstName for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadFirstNameForAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.FirstName = "!@$%#";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userUpdated = this.auctionUserService.GetAuctionUserByFistNameAndLastName("!@$%#", "Pascu");

            Assert.IsTrue(user != null && userUpdated == null);
        }

        /// <summary>
        /// Test update lastName for auction user.
        /// </summary>
        [Test]
        public void TestUpdateLastNameForAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.LastName = "Marcus";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userUpdated = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Marcus");

            Assert.IsTrue(user == null && userUpdated != null);
        }

        /// <summary>
        /// Test update firstName for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadLastNameForAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.LastName = "!@$%#";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userUpdated = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "!@$%#");

            Assert.IsTrue(user != null && userUpdated == null);
        }

        /// <summary>
        /// Test update gender for auction user.
        /// </summary>
        [Test]
        public void TestUpdateGenderForAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Gender = "F";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            Assert.IsTrue(user.Gender == "F");
        }

        /// <summary>
        /// Test update with bag gender for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadGenderForAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Gender = "C";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            Assert.IsTrue(user.Gender == "M");
        }

        /// <summary>
        /// Test update role for auction user.
        /// </summary>
        [Test]
        public void TestUpdateRoleForAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Role = Role.Seller.Value;
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            Assert.IsTrue(user.Role == Role.Seller.Value);
        }

        /// <summary>
        /// Test update with bad role for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadRoleForAuctionUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Role = "Customer";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            Assert.IsTrue(user.Role == Role.Buyer.Value);
        }

        /// <summary>
        /// Test delete User.
        /// </summary>
        [Test]
        public void TestDeleteUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var deleteResult = this.auctionUserService.DeleteAuctionUser(user.Id);
            Assert.True(!this.libraryContextMock.AuctionUsers.Any());
        }

        /// <summary>
        /// Test delete User.
        /// </summary>
        [Test]
        public void TestDeleteUserWithAWrongId()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var deleteResult = this.auctionUserService.DeleteAuctionUser(user.Id + 1);
            Assert.True(this.libraryContextMock.AuctionUsers.Count() == 1);
        }

        /// <summary>
        /// Test get user score without reviews.
        /// </summary>
        [Test]
        public void TestGetUserScoreWithoutReviews()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userScore = this.auctionUserService.GetAuctionUserScore(user.Id);
            var defaultScore = int.Parse(ConfigurationManager.AppSettings["DEFAULT_SCORE"]);
            Assert.True(userScore == defaultScore);
        }

        /// <summary>
        /// Test get user score without reviews.
        /// </summary>
        [Test]
        public void TestGetUserScoreWithReviews()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var reviewUser = new AuctionUser {FirstName = "Popa", LastName = "Andrei", Gender = "M"};
            var resultUser1 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
                {Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1};
            var review2 = new UserReview
                {Description = "He is a bad man", Score = 2, ReviewByUser = user2, ReviewForUser = user1};
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var resultReview2 = this.userReviewService.AddUserReview(review2);
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userScore = this.auctionUserService.GetAuctionUserScore(user.Id);
            var correctScore = review1.Score + review2.Score / 2;
            Assert.True(userScore == correctScore);
        }

        /// <summary>
        /// Test get user score without reviews.
        /// </summary>
        [Test]
        public void TestGetUserScoreWithReviewsDontHaveDefaultScore()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var reviewUser = new AuctionUser {FirstName = "Popa", LastName = "Andrei", Gender = "M"};
            var resultUser1 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
                {Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1};
            var review2 = new UserReview
                {Description = "He is a bad man", Score = 2, ReviewByUser = user2, ReviewForUser = user1};
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var resultReview2 = this.userReviewService.AddUserReview(review2);
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userScore = this.auctionUserService.GetAuctionUserScore(user.Id);
            var defaultScore = int.Parse(ConfigurationManager.AppSettings["DEFAULT_SCORE"]);
            Assert.True(userScore != defaultScore);
        }
    }
}