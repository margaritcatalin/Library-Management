// <copyright file="AuctionService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the auction service class.</summary>

namespace LibraryManagement.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The Auction service.
    /// </summary>
    public class AuctionService
    {
        /// <summary>
        /// Defines the auctionRepository.
        /// </summary>
        private readonly AuctionRepository auctionRepository;

        /// <summary>
        /// Defines the categoryService.
        /// </summary>
        private readonly CategoryService categoryService;

        /// <summary>
        /// Defines the auctionUserService.
        /// </summary>
        private readonly AuctionUserService auctionUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionService"/> class.
        /// </summary>
        /// <param name="auctionRepository">The Auction repository.</param>
        /// <param name="categoryService">The Auction category Service.</param>
        /// <param name="auctionUserService">The Auction user Service.</param>
        public AuctionService(AuctionRepository auctionRepository, CategoryService categoryService, AuctionUserService auctionUserService)
        {
            this.auctionRepository = auctionRepository;
            this.categoryService = categoryService;
            this.auctionUserService = auctionUserService;
        }

        /// <summary>
        /// Add a new Auction.
        /// </summary>
        /// <param name="auction">The Auction.</param>
        /// <returns>If Auction was added.</returns>
        public bool AddAuction(Auction auction)
        {
            if (this.ValidateAuction(auction))
            {
                if (!this.CheckIfUserCanOpenANewAuction(auction.Auctioneer))
                {
                    LoggerUtil.LogInfo($"Your score is too small.You need to wait some days.", MethodBase.GetCurrentMethod());
                    return false;
                }

                if (this.CheckIfUserHasMaxNumberOfStartedAuction(auction.Auctioneer))
                {
                    LoggerUtil.LogInfo($"You already have a lot of started auctions.", MethodBase.GetCurrentMethod());
                    return false;
                }

                var productsCategories =
                    this.categoryService.GetAllCategoriesProduct(auction.Product);
                if (this.CheckIfUserHasMaxNumberOfStartedAuctionInCategory(auction.Auctioneer, productsCategories))
                {
                    LoggerUtil.LogInfo($"You already have a lot of started auctions in this category.", MethodBase.GetCurrentMethod());
                    return false;
                }

                return this.auctionRepository.AddAuction(auction);
            }

            return false;
        }

        /// <summary>
        /// Get All Auction started by an specific user.
        /// </summary>
        /// <param name="user">The user what will be checked.</param>
        /// <returns>All Auction started by a user.</returns>
        public IEnumerable<Auction> GetStartedAuctionsByUser(AuctionUser user)
        {
            var auctions = this.GetAuctions();
            var filteredAuctions =
                from auction in auctions
                where auction.Auctioneer.Id == user.Id && !this.CheckIfAuctionIsEnded(auction)
                select auction;
            return filteredAuctions;
        }

        /// <summary>
        /// Get All Auction started by User in a specific category.
        /// </summary>
        /// <param name="user">The user what will be checked.</param>
        /// <param name="category">The category what will be checked.</param>
        /// <returns>All Auction started by User in a specific category.</returns>
        public IEnumerable<Auction> GetStartedAuctionsByUserAndCategory(AuctionUser user, Category category)
        {
            var auctions = this.GetStartedAuctionsByUser(user);

            var filteredAuctions =
                from auction in auctions
                where this.categoryService.GetAllCategoriesProduct(auction.Product).Contains(category)
                select auction;
            return filteredAuctions;
        }

        /// <summary>
        /// Get All AuctionUsers.
        /// </summary>
        /// <returns>All AuctionUsers.</returns>
        public IEnumerable<Auction> GetAuctions()
        {
            return this.auctionRepository.GetAuctions();
        }

        /// <summary>
        /// Get Auction by Auction id.
        /// </summary>
        /// <param name="id">The Auction id.</param>
        /// <returns>An Auction.</returns>
        public Auction GetAuctionById(int id)
        {
            return this.auctionRepository.GetAuctionById(id);
        }

        /// <summary>
        /// Update an Auction.
        /// </summary>
        /// <param name="auction">The Auction.</param>
        /// <returns>If Auction was updated.</returns>
        public bool UpdateAuction(Auction auction)
        {
            if (this.ValidateAuction(auction))
            {
                if (!auction.Bid.IsNullOrEmpty())
                {
                    Bid firstBid = auction.Bid.ToList()[0];
                    if (!auction.StartPrice.Currency.Equals(firstBid.BidPrice.Currency))
                    {
                        LoggerUtil.LogInfo($"This new start price is wrong.", MethodBase.GetCurrentMethod());
                        return false;
                    }
                }

                return this.auctionRepository.UpdateAuction(auction);
            }

            return false;
        }

        /// <summary>
        /// Delete Auction.
        /// </summary>
        /// <param name="id">The Auction id.</param>
        /// <returns>If Auction was deleted.</returns>
        public bool DeleteAuction(int id)
        {
            return this.auctionRepository.DeleteAuction(id);
        }

        /// <summary>
        /// Check if auction is ended.
        /// </summary>
        /// <param name="auction">The Auction.</param>
        /// <returns>If Auction is ended.</returns>
        public bool CheckIfAuctionIsEnded(Auction auction)
        {
            return DateTime.Compare(auction.StartDate, auction.EndDate) == 0 || auction.Ended;
        }

        /// <summary>
        /// Get last auction for user.
        /// </summary>
        /// <param name="auctionUser">The auctionUser.</param>
        /// <returns>Last started auction.</returns>
        public Auction GetLastAuctionForUser(AuctionUser auctionUser)
        {
            var allAuctions = this.GetAuctions();
            var filteredAuctions =
                from auction in allAuctions
                where auction.Auctioneer.Id == auctionUser.Id
                select auction;
            var orderedAuction = filteredAuctions.OrderBy(auction => auction.EndDate).ToList();
            if (orderedAuction.Any())
            {
                return orderedAuction.ToList()[orderedAuction.Count() - 1];
            }

            return null;
        }

        /// <summary>
        /// Get last auction price.
        /// </summary>
        /// <param name="auction">The auction.</param>
        /// <returns>Last auction price.</returns>
        public Price GetAuctionLastPrice(Auction auction)
        {
            var auctionBids = auction.Bid;
            if (auctionBids.IsNullOrEmpty())
            {
                return auction.StartPrice;
            }

            var orderedBids = auctionBids.OrderBy(bid => bid.BidDate).ToList();
            return orderedBids[orderedBids.Count - 1].BidPrice;
        }

        /// <summary>
        /// Check if auction is ended.
        /// </summary>
        /// <param name="auction">The Auction what will be ended.</param>
        /// <param name="user">The user how will end the auction.</param>
        /// <returns>If Auction was ended or not.</returns>
        public bool EndAuctionByUser(Auction auction, AuctionUser user)
        {
            if (auction.Auctioneer.Id != user.Id)
            {
                LoggerUtil.LogInfo($"This user couldn't end this auction.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (this.CheckIfAuctionIsEnded(auction))
            {
                LoggerUtil.LogInfo($"This auction is already ended.", MethodBase.GetCurrentMethod());
                return false;
            }

            auction.EndDate = DateTime.Now;
            auction.Ended = true;
            this.UpdateAuction(auction);
            return true;
        }

        /// <summary>
        /// Check if user has max number of started auction.
        /// </summary>
        /// <param name="auctionUser">The auctionUser.</param>
        /// <returns>If user has max number of started auction.</returns>
        private bool CheckIfUserHasMaxNumberOfStartedAuction(AuctionUser auctionUser)
        {
            var startedAuctions = this.GetStartedAuctionsByUser(auctionUser);
            var k = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_AUCTION"]);
            return startedAuctions.Count() == k;
        }

        /// <summary>
        /// Check if user has max number of started auction.
        /// </summary>
        /// <param name="auctionUser">The auctionUser.</param>
        /// <returns>If user has max number of started auction.</returns>
        private bool CheckIfUserCanOpenANewAuction(AuctionUser auctionUser)
        {
            var userScore = this.auctionUserService.GetAuctionUserScore(auctionUser.Id);
            var minScore = int.Parse(ConfigurationManager.AppSettings["MIN_SCORE"]);
            if (userScore < minScore)
            {
                var blockDays = int.Parse(ConfigurationManager.AppSettings["NUMBER_OF_BLOCK_DAYS"]);
                Auction lastAuction = this.GetLastAuctionForUser(auctionUser);
                if (lastAuction != null)
                {
                    var blockUntil = lastAuction.EndDate.AddDays(blockDays);
                    return DateTime.Compare(lastAuction.EndDate, blockUntil) > 0;
                } 
                else 
                {
                    LoggerUtil.LogInfo("User doesn't have an auction.", MethodBase.GetCurrentMethod());
                }
            } 
            else 
            {
                LoggerUtil.LogInfo("User is able to add an new auction.", MethodBase.GetCurrentMethod());
            }

            return true;
        }

        /// <summary>
        /// Check if user has max number of started auction in category.
        /// </summary>
        /// <param name="auctionUser">The auctionUser.</param>
        /// <param name="categories">The product categories.</param>
        /// <returns>If user has max number of started auction.</returns>
        private bool CheckIfUserHasMaxNumberOfStartedAuctionInCategory(AuctionUser auctionUser, IEnumerable<Category> categories)
        {
            var m = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_AUCTION_IN_CATEGORY"]);
            foreach (var category in categories)
            {
                var startedAuctions = this.GetStartedAuctionsByUserAndCategory(auctionUser, category);
                if (startedAuctions.Count() == m)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Validation for Auction user.
        /// </summary>
        /// <param name="auction">The Auction.</param>
        /// <returns>If Auction is valid or not.</returns>
        private bool ValidateAuction(Auction auction)
        {
            if (auction == null)
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null Auction.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (DateTime.Compare(auction.StartDate.Date, DateTime.Now.Date) < 0)
            {
                LoggerUtil.LogInfo($"Auction is invalid. Start date is need to be today or in the future.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (DateTime.Compare(auction.StartDate, auction.EndDate) >= 0)
            {
                LoggerUtil.LogInfo($"Auction is invalid. Start date is need to be less than end date.", MethodBase.GetCurrentMethod());
                return false;
            }

            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            if (DateTime.Compare(auction.StartDate.AddMonths(maxNumberOfMonths), auction.EndDate) < 0)
            {
                LoggerUtil.LogInfo($"Auction is invalid. End date >" + maxNumberOfMonths + ".", MethodBase.GetCurrentMethod());
                return false;
            }

            if (auction.StartPrice == null)
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null start price.", MethodBase.GetCurrentMethod());
                return false;
            }

            var minPrice = double.Parse(ConfigurationManager.AppSettings["MIN_PRICE"]);

            if (auction.StartPrice.Value < minPrice)
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null start price.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (auction.Auctioneer == null)
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null auctioneer.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!auction.Auctioneer.Role.Equals(Role.Seller.Value))
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an wrong user role.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (auction.Product == null)
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null product.", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }
    }
}