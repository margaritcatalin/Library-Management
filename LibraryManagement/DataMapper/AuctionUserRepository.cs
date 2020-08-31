// <copyright file="AuctionUserRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the AuctionUser repository class.</summary>

namespace LibraryManagement.DataMapper
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The AuctionUser repository.
    /// </summary>
    public class AuctionUserRepository
    {
        /// <summary>
        /// Defines the libraryContext.
        /// </summary>
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionUserRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The database manager.</param>
        public AuctionUserRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Get AuctionUser by first name and last name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>An AuctionUser.</returns>
        public AuctionUser GetAuctionUser(string firstName, string lastName)
        {
            return this.libraryContext.AuctionUsers.FirstOrDefault(
                a => a.FirstName.Equals(firstName) && a.LastName.Equals(lastName));
        }

        /// <summary>
        /// Add a new auctionUser.
        /// </summary>
        /// <param name="auctionUser">The new auctionUser.</param>
        /// <returns>If auctionUser was added.</returns>
        public bool AddAuctionUser(AuctionUser auctionUser)
        {
            this.libraryContext.AuctionUsers.Add(auctionUser);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"AuctionUser added successfully : {auctionUser.FirstName} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"AuctionUser failed to add to database : {auctionUser.FirstName}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get All AuctionUsers.
        /// </summary>
        /// <returns>All AuctionUsers.</returns>
        public IEnumerable<AuctionUser> GetAuctionUsers()
        {
            return this.libraryContext.AuctionUsers.ToList();
        }

        /// <summary>
        /// Get AuctionUser by AuctionUser id.
        /// </summary>
        /// <param name="id">The AuctionUser id.</param>
        /// <returns>An AuctionUser.</returns>
        public AuctionUser GetAuctionUserById(int id)
        {
            return this.libraryContext.AuctionUsers.Find(id);
        }

        /// <summary>
        /// Update an AuctionUser.
        /// </summary>
        /// <param name="auctionUser">The AuctionUser.</param>
        /// <returns>If auctionUser was updated.</returns>
        public bool UpdateAuctionUser(AuctionUser auctionUser)
        {
            this.libraryContext.Entry(auctionUser).State = EntityState.Modified;
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"AuctionUser was updated successfully : {auctionUser.FirstName} ", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Delete auctionUser.
        /// </summary>
        /// <param name="id">The auctionUser id.</param>
        /// <returns>If auctionUser was deleted.</returns>
        public bool DeleteAuctionUser(int id)
        {
            var auctionUser = this.libraryContext.AuctionUsers.Find(id);
            if (auctionUser != null)
            {
                this.libraryContext.AuctionUsers.Remove(auctionUser);
            }
            else
            {
                LoggerUtil.LogWarning($"AuctionUser failed to delete from database. We don't found an user with id: {id}", MethodBase.GetCurrentMethod());
                return false;
            }

            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"AuctionUser was deleted successfully : {id} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"AuctionUser failed to delete from database : {id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }
    }
}