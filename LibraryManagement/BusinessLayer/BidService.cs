// <copyright file="BidService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Bid service class.</summary>

namespace LibraryManagement.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The Bid service.
    /// </summary>
    public class BidService
    {
        /// <summary>
        /// Defines the bidRepository.
        /// </summary>
        private readonly BidRepository bidRepository;

        /// <summary>
        /// Defines the auctionService.
        /// </summary>
        private readonly AuctionService auctionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BidService"/> class.
        /// </summary>
        /// <param name="bidRepository">The Bid repository.</param>
        /// <param name="auctionService">The Bid service.</param>
        public BidService(BidRepository bidRepository, AuctionService auctionService)
        {
            this.bidRepository = bidRepository;
            this.auctionService = auctionService;
        }

        /// <summary>
        /// Add a new Bid in the database.
        /// </summary>
        /// <param name="bid">The Bid what will be added.</param>
        /// <returns>If Bid was added.</returns>
        public bool AddBid(Bid bid)
        {
            if (this.ValidateBid(bid))
            {
                return this.bidRepository.AddBid(bid);
            }

            return false;
        }

        /// <summary>
        /// Get All AuctionUsers.
        /// </summary>
        /// <returns>All AuctionUsers.</returns>
        public IEnumerable<Bid> GetBids()
        {
            return this.bidRepository.GetBids();
        }

        /// <summary>
        /// Get a Bid by Bid id.
        /// </summary>
        /// <param name="id">The Bid code.</param>
        /// <returns>If bid exist will be return the object if not will return null.</returns>
        public Bid GetBidById(int id)
        {
            return this.bidRepository.GetBidById(id);
        }

        /// <summary>
        /// Update an Bid.
        /// </summary>
        /// <param name="bid">The Bid what will be updated.</param>
        /// <returns>If Bid was updated.</returns>
        public bool UpdateBid(Bid bid)
        {
            if (this.ValidateBid(bid))
            {
                return this.bidRepository.UpdateBid(bid);
            }

            return false;
        }

        /// <summary>
        /// Delete Bid.
        /// </summary>
        /// <param name="id">The Bid id.</param>
        /// <returns>If Bid was deleted.</returns>
        public bool DeleteBid(int id)
        {
            return this.bidRepository.DeleteBid(id);
        }

        /// <summary>
        /// Validation for Bid user.
        /// </summary>
        /// <param name="bid">The Bid what will be validate.</param>
        /// <returns>If Bid is valid or not.</returns>
        private bool ValidateBid(Bid bid)
        {
            if (bid == null)
            {
                LoggerUtil.LogInfo($"Bid is invalid. You tried to add an null Bid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (bid.Auction == null)
            {
                LoggerUtil.LogInfo($"Bid is invalid. You tried to add an null auction.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (DateTime.Compare(bid.BidDate.Date, DateTime.Now.Date) != 0)
            {
                LoggerUtil.LogInfo($"Bid is invalid. Bid date is need to be today.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (bid.BidPrice == null)
            {
                LoggerUtil.LogInfo($"Bid is invalid. You tried to add an null price.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (bid.BidPrice != null && !bid.BidPrice.Currency.Equals(bid.Auction.StartPrice.Currency))
            {
                LoggerUtil.LogInfo($"Bid is invalid. You need to have a price in the auction currency.", MethodBase.GetCurrentMethod());
                return false;
            }

            var lastAuctionPrice = this.auctionService.GetAuctionLastPrice(bid.Auction);
            var bidPrice = bid.BidPrice;
            var valuePercent = 10 * lastAuctionPrice.Value / 100;
            if (bidPrice.Value < lastAuctionPrice.Value)
            {
                LoggerUtil.LogInfo($"Bid is invalid.You need to add a price > last price.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (bidPrice.Value > lastAuctionPrice.Value + valuePercent)
            {
                LoggerUtil.LogInfo($"Bid is invalid.You need to add a price <=10% by last price.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (bid.BidUser == null)
            {
                LoggerUtil.LogInfo($"Bid is invalid. You tried to add a bid with null user.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!bid.BidUser.Role.Equals(Role.Buyer.Value))
            {
                LoggerUtil.LogInfo($"Bid is invalid. You are a seller not a buyer.", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }
    }
}