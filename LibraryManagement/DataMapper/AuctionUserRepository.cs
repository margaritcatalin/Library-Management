// <copyright file="AuctionUserRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;
using LibraryManagement.DomainModel;

namespace LibraryManagement.DataMapper
{
    using LibraryManagement.Util;
    using System.Linq;

    /// <summary>
    /// The AuctionUser repository.
    /// </summary>
    public class AuctionUserRepository
    {
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionUserRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">Tha database manager.</param>
        public AuctionUserRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Get AuctionUser by firstname and lastname.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>An AuctionUser.</returns>
        public AuctionUser GetAuctionUser(string firstName, string lastName)
        {
            var entity = this.libraryContext.AuctionUsers.FirstOrDefault(
                a => a.FirstName.Equals(firstName) && a.LastName.Equals(lastName));
            DiscardChangesUtil.UndoingChangesDbEntityLevel(this.libraryContext, entity);
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
                LoggerUtil.LogInfo($"AuctionUser added successfully : {auctionUser.FirstName} ",
                    MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"AuctionUser failed to add to db : {auctionUser.FirstName}",
                    MethodBase.GetCurrentMethod());
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
            var entity = this.libraryContext.AuctionUsers.Find(id);
            DiscardChangesUtil.UndoingChangesDbEntityLevel(this.libraryContext, entity);
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
                LoggerUtil.LogInfo($"AuctionUser was updated successfully : {auctionUser.FirstName} ",
                    MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"AuctionUser failed to update to db : {auctionUser.FirstName}",
                    MethodBase.GetCurrentMethod());
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
                LoggerUtil.LogError($"AuctionUser failed to delete from db. We don't found an user with id: {id}",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"AuctionUser was deleted successfully : {id} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"AuctionUser failed to delete from db : {id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }
    }
}