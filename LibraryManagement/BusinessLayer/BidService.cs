// <copyright file="BidService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System;
using System.Collections.Generic;
using LibraryManagement.DomainModel;
using LibraryManagement.Util;

namespace LibraryManagement.BusinessLayer
{
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;

    /// <summary>
    /// The Bid service.
    /// </summary>
    public class BidService
    {
        private readonly BidRepository bidRepository;
        private readonly AuctionService auctionService;

        /// <summary>Initializes a new instance of the <see cref="BidRepository"/> class.</summary>
        /// <param name="bidRepository">The Bid repository.</param>
        /// <param name="auctionService">The Bid service.</param>
        public BidService(BidRepository bidRepository, AuctionService auctionService)
        {
            this.bidRepository = bidRepository;
            this.auctionService = auctionService;
        }
        
        
        /// <summary>
        /// Add a new Bid.
        /// </summary>
        /// <param name="bid">The Bid.</param>
        /// <returns>If Bid was added.</returns>
        public bool AddBid(Bid bid)
        {
            if (ValidateBid(bid))
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
        /// Get Bid by Bid id.
        /// </summary>
        /// <param name="id">The Bid id.</param>
        /// <returns>An Bid.</returns>
        public Bid GetBidById(int id)  
        {
            return this.bidRepository.GetBidById(id);  
        }  
        
        /// <summary>
        /// Update an Bid.
        /// </summary>
        /// <param name="bid">The Bid.</param>
        /// <returns>If Bid was updated.</returns>
        public bool UpdateBid(Bid bid)  
        {
            if (ValidateBid(bid))
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
        /// <param name="bid">The Bid.</param>
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
                LoggerUtil.LogInfo($"Bid is invalid. Bid date is need to be today.",
                    MethodBase.GetCurrentMethod());
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
            if (bidPrice.Value < lastAuctionPrice.Value + valuePercent)
            {
                LoggerUtil.LogInfo($"Bid is invalid.You need to add a price >10% by last price.", MethodBase.GetCurrentMethod());
                return false;
            }
            if (bid.BidUser==null)
            {
                LoggerUtil.LogInfo($"Bid is invalid. You tried to add a bid with null user.", MethodBase.GetCurrentMethod());
                return false;
            }
            if (!bid.BidUser.Role.Equals(Role.Buyer.Value))
            {
                LoggerUtil.LogInfo($"Bid is invalid. You are a seller not a buyer.",
                    MethodBase.GetCurrentMethod());
                return false;
            }
            return true;
        }
    }
}