// <copyright file="AuctionUserService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the auction user service class.</summary>

namespace LibraryManagement.BusinessLayer
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The AuctionUser service.
    /// </summary>
    public class AuctionUserService
    {
        /// <summary>
        /// Defines the auctionUserRepository.
        /// </summary>
        private readonly AuctionUserRepository auctionUserRepository;

        /// <summary>
        /// Defines the userReviewService.
        /// </summary>
        private readonly UserReviewService userReviewService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionUserService"/> class.
        /// </summary>
        /// <param name="auctionUserRepository">The AuctionUser repository.</param>
        /// <param name="userReviewService">The userReview Service .</param>
        public AuctionUserService(AuctionUserRepository auctionUserRepository, UserReviewService userReviewService)
        {
            this.auctionUserRepository = auctionUserRepository;
            this.userReviewService = userReviewService;
        }

        /// <summary>
        /// Add a new AuctionUser with a specific role.
        /// </summary>
        /// <param name="auctionUser">The auctionUser what is need to be added.</param>
        /// <param name="role">The role for the user.</param>
        /// <returns>If auctionUser was added or not.</returns>
        public bool AddAuctionUser(AuctionUser auctionUser, Role role)
        {
            if (role == null)
            {
                LoggerUtil.LogInfo($"UserRole is invalid. You tried to add an userRole with null empty role.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (auctionUser == null)
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an null auctionUser.", MethodBase.GetCurrentMethod());
                return false;
            }

            auctionUser.Role = role.Value;

            if (this.ValidateAuctionUser(auctionUser))
            {
                return this.auctionUserRepository.AddAuctionUser(auctionUser);
            }

            return false;
        }

        /// <summary>
        /// Get All AuctionUsers.
        /// </summary>
        /// <returns>All AuctionUsers.</returns>
        public IEnumerable<AuctionUser> GetAuctionUsers()
        {
            return this.auctionUserRepository.GetAuctionUsers();
        }

        /// <summary>
        /// Get AuctionUser by AuctionUser id.
        /// </summary>
        /// <param name="id">The AuctionUser id.</param>
        /// <returns>An AuctionUser.</returns>
        public AuctionUser GetAuctionUserById(int id)
        {
            return this.auctionUserRepository.GetAuctionUserById(id);
        }

        /// <summary>
        /// Get AuctionUser by first name and last name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>An AuctionUser.</returns>
        public AuctionUser GetAuctionUserByFistNameAndLastName(string firstName, string lastName)
        {
            if (firstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Param firstName is required.", MethodBase.GetCurrentMethod());
                return null;
            }

            if (lastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Param lastName is required.", MethodBase.GetCurrentMethod());
                return null;
            }

            return this.auctionUserRepository.GetAuctionUser(firstName, lastName);
        }

        /// <summary>
        /// Update an AuctionUser.
        /// </summary>
        /// <param name="auctionUser">The AuctionUser.</param>
        /// <returns>If auctionUser was updated.</returns>
        public bool UpdateAuctionUser(AuctionUser auctionUser)
        {
            if (this.ValidateAuctionUser(auctionUser))
            {
                return this.auctionUserRepository.UpdateAuctionUser(auctionUser);
            }

            return false;
        }

        /// <summary>
        /// Delete auctionUser.
        /// </summary>
        /// <param name="id">The auctionUser id.</param>
        /// <returns>If auctionUser was deleted.</returns>
        public bool DeleteAuctionUser(int id)
        {
            return this.auctionUserRepository.DeleteAuctionUser(id);
        }

        /// <summary>
        /// Get auctionUser score.
        /// </summary>
        /// <param name="id">The auctionUser id.</param>
        /// <returns>Value of user score.</returns>
        public int GetAuctionUserScore(int id)
        {
            AuctionUser user = this.GetAuctionUserById(id);
            var userReviews = this.userReviewService.GetUserReviewsForUser(user);
            var defaultScore = int.Parse(ConfigurationManager.AppSettings["DEFAULT_SCORE"]);
            var averageOf = int.Parse(ConfigurationManager.AppSettings["SCORE_AVERAGE_OF"]);
            var enumerable = userReviews.ToList();
            if (enumerable.IsNullOrEmpty())
            {
                return defaultScore;
            }

            var sum = 0;
            if (enumerable.Count() < averageOf)
            {
                foreach (var review in enumerable)
                {
                    sum += review.Score;
                }

                return sum / enumerable.Count();
            }
            else
            {
                for (int index = 0; index <= averageOf; index++)
                {
                    sum += enumerable.ToArray()[index].Score;
                }

                return sum / averageOf;
            }
        }

        /// <summary>
        /// Validation for auction user.
        /// </summary>
        /// <param name="auctionUser">The AuctionUser.</param>
        /// <returns>If auctionUser is valid or not.</returns>
        private bool ValidateAuctionUser(AuctionUser auctionUser)
        {
            if (auctionUser.FirstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an author with null empty firstName.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((auctionUser.FirstName.Length < 3) || (auctionUser.FirstName.Length > 100))
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an auctionUser with invalid lenght firstName.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!auctionUser.FirstName.All(a => char.IsLetter(a) || char.IsWhiteSpace(a) || (a == '-')))
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an auctionUser with invalid character in FirstName.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(auctionUser.FirstName.First()))
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an auctionUser with firstName with lower case.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (auctionUser.LastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an auctionUser with last name null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((auctionUser.LastName.Length < 3) || (auctionUser.LastName.Length > 100))
            {
                LoggerUtil.LogInfo(
                    $"AuctionUser is invalid. You tried to add an auctionUser with invalid length for lastName.",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            if (!auctionUser.LastName.All(a => char.IsLetter(a) || char.IsWhiteSpace(a) || (a == '-')))
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an auctionUser with invalid characters.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(auctionUser.LastName.First()))
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an auctionUser with lower case LastName.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (auctionUser.Gender.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an auctionUser with null or empty gender.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!(auctionUser.Gender.Equals("M") || auctionUser.Gender.Equals("F")))
            {
                LoggerUtil.LogInfo($"AuctionUser is invalid. You tried to add an author with invalid gender.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!this.ValidateUserRoleName(auctionUser.Role))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The Validation for a role name.
        /// </summary>
        /// <param name="roleName">The role name string.</param>
        /// <returns>If role name is valid or not.</returns>
        private bool ValidateUserRoleName(string roleName)
        {
            if (roleName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"UserRole is invalid. You tried to add an userRole with null empty role.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(roleName.First()))
            {
                LoggerUtil.LogInfo($"UserRole is invalid. You tried to add an userRole with role with lower case.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!roleName.Equals(Role.Buyer.Value) && !roleName.Equals(Role.Seller.Value))
            {
                LoggerUtil.LogInfo($"UserRole is invalid. You tried to add an userRole with invalid role.", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }
    }
}