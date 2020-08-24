// <copyright file="UserReviewService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the UserReview service class.</summary>

namespace LibraryManagement.BusinessLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The UserReview service.
    /// </summary>
    public class UserReviewService
    {
        /// <summary>
        /// Defines the userReviewRepository.
        /// </summary>
        private readonly UserReviewRepository userReviewRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserReviewService"/> class.
        /// </summary>
        /// <param name="userReviewRepository">The UserReview repository.</param>
        public UserReviewService(UserReviewRepository userReviewRepository)
        {
            this.userReviewRepository = userReviewRepository;
        }

        /// <summary>
        /// Add a new UserReview.
        /// </summary>
        /// <param name="userReview">The UserReview.</param>
        /// <returns>If UserReview was added.</returns>
        public bool AddUserReview(UserReview userReview)
        {
            if (this.ValidateUserReview(userReview))
            {
                return this.userReviewRepository.AddUserReview(userReview);
            }

            return false;
        }

        /// <summary>
        /// Get All UserReviews for User.
        /// </summary>
        /// <param name="user">The user<see cref="AuctionUser"/>.</param>
        /// <returns>All all user reviews for user.</returns>
        public IEnumerable<UserReview> GetUserReviewsForUser(AuctionUser user)
        {
            if (user == null)
            {
                LoggerUtil.LogInfo($"Param user is required.", MethodBase.GetCurrentMethod());
                return null;
            }

            var allUserReviews = this.GetUserReviews();
            var filteredUserReviews = from review in allUserReviews
                                      where review.ReviewForUser.Id == user.Id
                                      select review;
            return filteredUserReviews;
        }

        /// <summary>
        /// Get All AuctionUsers.
        /// </summary>
        /// <returns>All AuctionUsers.</returns>
        public IEnumerable<UserReview> GetUserReviews()
        {
            return this.userReviewRepository.GetUserReviews();
        }

        /// <summary>
        /// Get UserReview by UserReview id.
        /// </summary>
        /// <param name="id">The UserReview id.</param>
        /// <returns>An UserReview.</returns>
        public UserReview GetUserReviewById(int id)
        {
            return this.userReviewRepository.GetUserReviewById(id);
        }

        /// <summary>
        /// Update an UserReview.
        /// </summary>
        /// <param name="userReview">The UserReview.</param>
        /// <returns>If UserReview was updated.</returns>
        public bool UpdateUserReview(UserReview userReview)
        {
            if (this.ValidateUserReview(userReview))
            {
                return this.userReviewRepository.UpdateUserReview(userReview);
            }

            return false;
        }

        /// <summary>
        /// Delete UserReview.
        /// </summary>
        /// <param name="id">The UserReview id.</param>
        /// <returns>If UserReview was deleted.</returns>
        public bool DeleteUserReview(int id)
        {
            return this.userReviewRepository.DeleteUserReview(id);
        }

        /// <summary>
        /// Validation for UserReview user.
        /// </summary>
        /// <param name="userReview">The UserReview.</param>
        /// <returns>If UserReview is valid or not.</returns>
        private bool ValidateUserReview(UserReview userReview)
        {
            if (userReview == null)
            {
                LoggerUtil.LogInfo($"UserReview is invalid. You tried to add an null UserReview.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (userReview.ReviewForUser == null)
            {
                LoggerUtil.LogInfo($"UserReview is invalid. For User is required.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (userReview.ReviewByUser == null)
            {
                LoggerUtil.LogInfo($"UserReview is invalid. By User is required.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (userReview.ReviewByUser.Id == userReview.ReviewForUser.Id)
            {
                LoggerUtil.LogInfo($"UserReview is invalid. You are not able to add a review for yourself.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (userReview.Score < 1 || userReview.Score > 99)
            {
                LoggerUtil.LogInfo($"UserReview is invalid. Score is need to be more or equals with 1", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!this.ValidateDescription(userReview.Description))
            {
                LoggerUtil.LogInfo($"UserReview is invalid. Invalid description", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validation for description .
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>If UserReview is valid or not.</returns>
        private bool ValidateDescription(string description)
        {
            if (description.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Description is invalid. You tried to add an userRole with null empty description.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((description.Length < 3) || (description.Length > 100))
            {
                LoggerUtil.LogInfo(
                    $"Description is invalid. You tried to add an userRole with invalid length description.", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }
    }
}