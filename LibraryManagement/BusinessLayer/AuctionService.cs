// <copyright file="AuctionService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System;
using System.Collections.Generic;
using System.Configuration;
using LibraryManagement.DomainModel;
using LibraryManagement.Util;

namespace LibraryManagement.BusinessLayer
{
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using DataMapper;

    /// <summary>
    /// The Auction service.
    /// </summary>
    public class AuctionService
    {
        private readonly AuctionRepository auctionRepository;
        private readonly CategoryService categoryService;
        private readonly AuctionUserService auctionUserService;

        /// <summary>Initializes a new instance of the <see cref="AuctionRepository"/> class.</summary>
        /// <param name="auctionRepository">The Auction repository.</param>
        /// <param name="categoryService">The Auction category Service.</param>
        /// <param name="auctionUserService">The Auction user Service.</param>
        public AuctionService(AuctionRepository auctionRepository, CategoryService categoryService,
            AuctionUserService auctionUserService)
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
            if (ValidateAuction(auction))
            {
                if (!CheckIfUserCanOpenANewAuction(auction.Auctioneer))
                {
                    LoggerUtil.LogInfo($"Your score is too small.You need to wait some days.",
                        MethodBase.GetCurrentMethod());
                    return false;
                }

                if (CheckIfUserHasMaxNumberOfStartedAuction(auction.Auctioneer))
                {
                    LoggerUtil.LogInfo($"You already have a lot of started auctions.", MethodBase.GetCurrentMethod());
                    return false;
                }

                var productsCategories =
                    categoryService.GetAllCategoriesProduct(auction.Product);
                if (CheckIfUserHasMaxNumberOfStartedAuctionInCategory(auction.Auctioneer, productsCategories))
                {
                    LoggerUtil.LogInfo($"You already have a lot of started auctions in this category.",
                        MethodBase.GetCurrentMethod());
                    return false;
                }

                return auctionRepository.AddAuction(auction);
            }

            return false;
        }

        /// <summary>
        /// Get All Auction started by User.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>All Auction started by user.</returns>
        public IEnumerable<Auction> GetStartedAuctionsByUser(AuctionUser user)
        {
            var auctions = GetAuctions();
            var filteredAuctions =
                from auction in auctions
                where auction.Auctioneer.Id == user.Id && !CheckIfAuctionIsEnded(auction)
                select auction;
            return filteredAuctions;
        }

        /// <summary>
        /// Get All Auction started by User in a specific category.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="category">The category.</param>
        /// <returns>All Auction started by User in a specific category.</returns>
        public IEnumerable<Auction> GetStartedAuctionsByUserAndCategory(AuctionUser user, Category category)
        {
            var auctions = GetStartedAuctionsByUser(user);

            var filteredAuctions =
                from auction in auctions
                where categoryService.GetAllCategoriesProduct(auction.Product).Contains(category)
                select auction;
            return filteredAuctions;
        }

        /// <summary>
        /// Get All AuctionUsers.
        /// </summary>
        /// <returns>All AuctionUsers.</returns>
        public IEnumerable<Auction> GetAuctions()
        {
            return auctionRepository.GetAuctions();
        }

        /// <summary>
        /// Get Auction by Auction id.
        /// </summary>
        /// <param name="id">The Auction id.</param>
        /// <returns>An Auction.</returns>
        public Auction GetAuctionById(int id)
        {
            return auctionRepository.GetAuctionById(id);
        }

        /// <summary>
        /// Update an Auction.
        /// </summary>
        /// <param name="auction">The Auction.</param>
        /// <returns>If Auction was updated.</returns>
        public bool UpdateAuction(Auction auction)
        {
            if (ValidateAuction(auction))
            {
                if (!auction.Bids.IsNullOrEmpty())
                {
                    Bid firstBid = auction.Bids.ToList()[0];
                    if (!auction.StartPrice.Currency.Equals(firstBid.BidPrice.Currency))
                    {
                        LoggerUtil.LogInfo($"This new start price is wrong.", MethodBase.GetCurrentMethod());
                        return false;
                    }
                }

                return auctionRepository.UpdateAuction(auction);
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
            return auctionRepository.DeleteAuction(id);
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
        /// Check if user has max number of started auction.
        /// </summary>
        /// <param name="auctionUser">The auctionUser.</param>
        /// <returns>If user has max number of started auction.</returns>
        private bool CheckIfUserHasMaxNumberOfStartedAuction(AuctionUser auctionUser)
        {
            var startedAuctions = GetStartedAuctionsByUser(auctionUser);
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
                Auction lastAuction = GetLastAuctionForUser(auctionUser);
                if (lastAuction != null)
                {
                    var blockUntil = lastAuction.EndDate.AddDays(blockDays);
                    return DateTime.Compare(lastAuction.EndDate, blockUntil) > 0;
                }
            }

            return true;
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
            var auctionBids = auction.Bids;
            if (auctionBids.IsNullOrEmpty())
            {
                return auction.StartPrice;
            }

            var orderedBids = auctionBids.OrderBy(bid => bid.BidDate).ToList();
            return orderedBids[orderedBids.Count - 1].BidPrice;
        }

        /// <summary>
        /// Check if user has max number of started auction in category.
        /// </summary>
        /// <param name="auctionUser">The auctionUser.</param>
        /// <param name="categories">The product categories.</param>
        /// <returns>If user has max number of started auction.</returns>
        private bool CheckIfUserHasMaxNumberOfStartedAuctionInCategory(AuctionUser auctionUser,
            IEnumerable<Category> categories)
        {
            var m = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_AUCTION_IN_CATEGORY"]);
            foreach (var category in categories)
            {
                var startedAuctions = GetStartedAuctionsByUserAndCategory(auctionUser, category);
                if (startedAuctions.Count() == m)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if auction is ended.
        /// </summary>
        /// <param name="auction">The Auction.</param>
        /// <param name="user">The user.</param>
        /// <returns>If Auction was ended.</returns>
        public bool EndAuctionByUser(Auction auction, AuctionUser user)
        {
            if (auction.Auctioneer.Id != user.Id)
            {
                LoggerUtil.LogInfo($"This user couldn't end this auction.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (CheckIfAuctionIsEnded(auction))
            {
                LoggerUtil.LogInfo($"This auction is already ended.", MethodBase.GetCurrentMethod());
                return false;
            }

            auction.EndDate = DateTime.Now;
            auction.Ended = true;
            UpdateAuction(auction);
            return true;
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
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null Auction.",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            if (DateTime.Compare(auction.StartDate.Date, DateTime.Now.Date) < 0)
            {
                LoggerUtil.LogInfo($"Auction is invalid. Start date is need to be today or in the future.",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            if (DateTime.Compare(auction.StartDate, auction.EndDate) >= 0)
            {
                LoggerUtil.LogInfo($"Auction is invalid. Start date is need to be less than end date.",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            if (DateTime.Compare(auction.StartDate.AddMonths(maxNumberOfMonths), auction.EndDate) < 0)
            {
                LoggerUtil.LogInfo($"Auction is invalid. End date >" + maxNumberOfMonths + ".",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            if (auction.StartPrice == null)
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null start price.",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            var minPrice = double.Parse(ConfigurationManager.AppSettings["MIN_PRICE"]);

            if (auction.StartPrice.Value < minPrice)
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null start price.",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            if (auction.Auctioneer == null)
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null auctioneer.",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            if (!auction.Auctioneer.Role.Equals(Role.Seller.Value))
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an wrong user role.",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            if (auction.Product == null)
            {
                LoggerUtil.LogInfo($"Auction is invalid. You tried to add an null product.",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }
    }
}