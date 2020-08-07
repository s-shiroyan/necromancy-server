using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Database.Sql.Core
{
    class NecSqlDbAuction
    {
        private const string SqlInsertAuctionItem =
            "INSERT OR INTO nec_auction_items (character_id, item_id, quantity, expiry_time, min_bid, buyout_price, bidder_id, current_bid, comment) VALUES (@character_id, @item_id, @quantity, @expiry_time, @min_bid, @buyout_price, @bidder_id, @current_bid, @comment)";
        private const string SqlUpdateAuctionBid =
            "UPDATE nec_auction_items (bidder_id, current_bid) VALUES (@bidder_id, @current_bid) WHERE id = @id";
        private const string SqlSelectShortcutBar =
            "SELECT slot_num, shortcut_type, shortcut_id FROM nec_shortcut_bar WHERE character_id = @character_id AND bar_num = @bar_num";
    }
}
