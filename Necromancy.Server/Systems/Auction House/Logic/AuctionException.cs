using Necromancy.Server.Chat.Command.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House.Logic
{
    [Serializable()]
    public class AuctionException : Exception
    {
        public Type ExceptionType { get; private set; }
        public AuctionException() : base() { ExceptionType = Type.GENERIC_ERROR; }
        public AuctionException(Type exceptionType) : base() { ExceptionType = exceptionType; }
        public AuctionException(string message) : base(message) { }
        public AuctionException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected AuctionException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public enum Type
        {
            /// <summary>
            /// This item may not be listed.
            /// </summary>
            INVALID_LISTING = 1,
            /// <summary>
            /// You may not list the equipped items.
            /// </summary>
            EQUIP_LISTING = 2,
            /// <summary>
            /// No space available in item list.
            /// </summary>
            LOT_SLOTS_FULL = 3,
            /// <summary>
            /// The minimum price is lower than the Buy Now price.
            /// </summary>
            MIN_PRICE_LOWER_THAN_BUYOUT = 4,
            /// <summary>
            /// Please place a bid higher than the one you've already placed.
            /// </summary>
            NEW_BID_LOWER_THAN_PREV = 5,
            /// <summary>
            /// No space available in Bidding Item List.
            /// </summary>
            BID_SLOTS_FULL = 6,
            /// <summary>
            /// Unable to change bid value as Dimento Medal has expired.
            /// </summary>
            BID_DIMETO_MEDAL_EXPIRED = 7,
            /// <summary>
            /// Unable to list item as Dimento Medal has expired
            /// </summary>
            LOT_DIMETO_MEDAL_EXPIRED = 8,
            /// <summary>
            /// Illegal search query.\nPlease set a minimum and maximum amount.
            /// </summary>
            INVALID_SEARCH_QUERY = 9,
            /// <summary>
            /// Item has already been listed.
            /// </summary>
            ITEM_ALREADY_LIST = -3700,
            /// <summary>
            /// Illegal Status.
            /// </summary>
            ILLEGAL_STATUS = -3701,
            /// <summary>
            /// You have already bid on this item.
            /// </summary>
            ALREADY_BID = -3702,
            /// <summary>
            /// You are unable to cancel as there is less than one hour remaining.
            /// </summary>
            NO_CANCEL_EXPIRY_CLOSE = -3703,
            /// <summary>
            /// You are unable to cancel because another character has already bid.
            /// </summary>
            ANOTHER_CHARACTER_ALREADY_BID = -3704,
            /// <summary>
            /// Listing time over.
            /// </summary>
            LISTING_EXPIRED = -3705,
            /// <summary>
            /// Listing cancelled.
            /// </summary>
            LISTING_CANCELLED = -3706,
            /// <summary>
            /// Bidding has already completed.
            /// </summary>
            BIDDING_COMPLETED = -3707,
            /// <summary>
            /// Slot unavailable.
            /// </summary>
            SLOT_UNAVAILABLE = -203,
            /// <summary>
            /// Illegal item amount
            /// </summary>
            ILLEGAL_ITEM_AMOUNT = -204,
            /// <summary>
            /// Auction error.
            /// </summary>
            GENERIC_ERROR = 9999
        }
    }
}
