// <copyright file="UserReviewRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the UserReview repository class.</summary>

namespace LibraryManagement.DataMapper
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The UserReview repository.
    /// </summary>
    public class UserReviewRepository
    {
        /// <summary>
        /// Defines the libraryContext.
        /// </summary>
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserReviewRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The database manager.</param>
        public UserReviewRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new UserReview.
        /// </summary>
        /// <param name="userReview">The new UserReview.</param>
        /// <returns>If price was UserReview.</returns>
        public bool AddUserReview(UserReview userReview)
        {
            this.libraryContext.UserReviews.Add(userReview);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"UserReview added successfully : {userReview.Id}", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"UserReview failed to add to database : {userReview.Id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get All UserReviews.
        /// </summary>
        /// <returns>All UserReviews.</returns>
        public IEnumerable<UserReview> GetUserReviews()
        {
            return this.libraryContext.UserReviews.ToList();
        }

        /// <summary>
        /// Get UserReview by id.
        /// </summary>
        /// <param name="id">The UserReview id.</param>
        /// <returns>A UserReview.</returns>
        public UserReview GetUserReviewById(int id)
        {
            return this.libraryContext.UserReviews.Find(id);
        }

        /// <summary>
        /// Update a UserReview.
        /// </summary>
        /// <param name="userReview">The UserReview.</param>
        /// <returns>If UserReview was updated.</returns>
        public bool UpdateUserReview(UserReview userReview)
        {
            this.libraryContext.Entry(userReview).State = EntityState.Modified;
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Price updated successfully : {userReview.Id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Delete a UserReview.
        /// </summary>
        /// <param name="id">The UserReview id.</param>
        /// <returns>If UserReview was deleted.</returns>
        public bool DeleteUserReview(int id)
        {
            var userReview = this.libraryContext.UserReviews.Find(id);
            if (userReview != null)
            {
                this.libraryContext.UserReviews.Remove(userReview);
            }
            else
            {
                LoggerUtil.LogError($"Price failed to delete from database. We don't found a userReview with id: {id}", MethodBase.GetCurrentMethod());
                return false;
            }

            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Price was deleted successfully : {id} ", MethodBase.GetCurrentMethod());
            }

            return successful;
        }
    }
}