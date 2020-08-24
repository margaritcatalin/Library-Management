// <copyright file="UserReviewTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System.Linq;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;
    using NUnit.Framework;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The userReview unit tests.
    /// </summary>
    [TestFixture]
    public class UserReviewTests
    {
        /// <summary>
        /// Defines the libraryContextMock.
        /// </summary>
        private LibraryDbContext libraryContextMock;

        /// <summary>
        /// Defines the userReviewService.
        /// </summary>
        private UserReviewService userReviewService;

        /// <summary>
        /// Defines the auctionUserService.
        /// </summary>
        private AuctionUserService auctionUserService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            EntityFrameworkMock.PrepareMock(this.libraryContextMock);
            this.userReviewService = new UserReviewService(new UserReviewRepository(this.libraryContextMock));
            this.auctionUserService = new AuctionUserService(new AuctionUserRepository(this.libraryContextMock), this.userReviewService);
        }

        /// <summary>
        /// Test add a new userReview.
        /// </summary>
        [Test]
        public void TestAddUserReview()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(this.libraryContextMock.UserReviews.Count() == 1);
        }

        /// <summary>
        /// Test add a null userReview.
        /// </summary>
        [Test]
        public void TestAddNullUserReview()
        {
            var result = this.userReviewService.AddUserReview(null);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test add an userReview with null description.
        /// </summary>
        [Test]
        public void TestAddUserReviewWithNullDescription()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = null, Score = 4, ReviewByUser = user2, ReviewForUser = userReview };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test add an userReview with null review user.
        /// </summary>
        [Test]
        public void TestAddNullReviewByUserUserReview()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = null, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test add an userReview with null review to user.
        /// </summary>
        [Test]
        public void TestAddNullReviewForUserUserReview()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = userReview, ReviewForUser = null };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test add an userReview with zero score.
        /// </summary>
        [Test]
        public void TestAddZeroScoreUserReview()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 0, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test add an userReview with less than 0 score.
        /// </summary>
        [Test]
        public void TestAddLessThanZeroScoreUserReview()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = -3, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test add an userReview with a big score.
        /// </summary>
        [Test]
        public void TestAddBigScoreUserReview()
        {
            var userReview = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "F" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserById(userReview.Id);
            var user2 = this.auctionUserService.GetAuctionUserById(reviewUser.Id);
            var review1 = new UserReview
            { Description = "He is a good man", Score = 999, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test add a new userReview for you.
        /// </summary>
        [Test]
        public void TestAddUserReviewForYourself()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user1, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test add userReview with smaller firstName.
        /// </summary>
        [Test]
        public void TestAddSmallerDescriptionUserReview()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "nu", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test add userReview with longer firstName.
        /// </summary>
        [Test]
        public void TestAddLongerDescriptionUserReview()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            {
                Description =
                    "bFuDu3e8H7LuEGyZbYTUfxdKm9gweGGho3e8H9m2hlgBHgglrMME0Rf6CI99bdLg1wnSwBj3dKsJ2LpRENQIloUp4m7oj4Xs9mMbxbFuDu3e8H7LuEGyZbYTUfxdKm9gweGGho3e8H9m2hlgBHgglrMME0Rf6CI99bdLg1wnSwBj3dKsJ2LpRENQIloUp4m7oj4Xs9mMbx",
                Score = 4,
                ReviewByUser = user2,
                ReviewForUser = user1
            };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test get userReview by id and he is in the database.
        /// </summary>
        [Test]
        public void TestGetUserReviewByGoodId()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviews = this.userReviewService.GetUserReviews();
            var lastReviewId = reviews.ToList()[0].Id;
            var userReviewById = this.userReviewService.GetUserReviewById(lastReviewId);
            Assert.NotNull(userReviewById);
        }

        /// <summary>
        /// Test get userReview by id and he is not in the database.
        /// </summary>
        [Test]
        public void TestGetUserReviewByBadId()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var userReviewById = this.userReviewService.GetUserReviewById(888);
            Assert.Null(userReviewById);
        }

        /// <summary>
        /// Test get all userReviews.
        /// </summary>
        [Test]
        public void TestGetAllUserReviews()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var review2 = new UserReview
            { Description = "He is a man", Score = 2, ReviewByUser = userReview, ReviewForUser = user2 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var resultReview2 = this.userReviewService.AddUserReview(review2);

            var reviews = this.userReviewService.GetUserReviews();
            Assert.IsTrue(reviews.Count() == 2);
        }

        /// <summary>
        /// Test get all userReviews with a wrong review.
        /// </summary>
        [Test]
        public void TestGetAllUserReviewsWithAWrongUser()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var review2 = new UserReview
            { Description = null, Score = 2, ReviewByUser = userReview, ReviewForUser = user2 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var resultReview2 = this.userReviewService.AddUserReview(review2);

            var reviews = this.userReviewService.GetUserReviews();
            Assert.IsFalse(reviews.Count() == 2);
        }

        /// <summary>
        /// Test delete UserReview.
        /// </summary>
        [Test]
        public void TestDeleteUserReview()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Seller);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");
            var review1 = new UserReview
            { Id = 3, Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviews = this.userReviewService.GetUserReviews();
            var lastReview = reviews.ToList()[0];
            var deleteResult = this.userReviewService.DeleteUserReview(lastReview.Id);
            Assert.True(!this.libraryContextMock.UserReviews.Any());
        }

        /// <summary>
        /// Test delete UserReview.
        /// </summary>
        [Test]
        public void TestDeleteUserReviewWithAWrongId()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviews = this.userReviewService.GetUserReviews();
            var lastReview = reviews.ToList()[0];
            var deleteResult = this.userReviewService.DeleteUserReview(lastReview.Id + 1);
            Assert.True(this.libraryContextMock.UserReviews.Count() == 1);
        }

        /// <summary>
        /// Test delete UserReview.
        /// </summary>
        [Test]
        public void TestDeleteUserReviewWithANullUser()
        {
            var result = this.userReviewService.GetUserReviewsForUser(null);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test delete UserReview.
        /// </summary>
        [Test]
        public void TestGetUserReviewsForUser()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { Id = 2, FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviews = this.userReviewService.GetUserReviewsForUser(user1);
            Assert.True(reviews.Count() == 1);
        }

        /// <summary>
        /// Test delete UserReview.
        /// </summary>
        [Test]
        public void TestGetUserReviewsForUserEmptyReviews()
        {
            var userReview = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var reviews = this.userReviewService.GetUserReviewsForUser(user1);
            Assert.True(!reviews.Any());
        }
    }
}