using Necromancy.Server.Model;
using Necromancy.Server.Systems.Item;
using System;
using System.Collections.Generic;

namespace Necromancy.Server.Systems.Item
{
    public class ItemService
    {
        private readonly Character _character;
        private readonly IItemDao _itemDao;

        public enum MoveType
        {
            Place,
            Swap,
            PlaceQuantity,
            AddQuantity,
            AllQuantity,
            None
        }

        public ItemService(Character character)
        {
            _itemDao = new ItemDao();
            _character = character;
        }

        public ItemService(Character character, IItemDao itemDao)
        {
            _itemDao = itemDao;
            _character = character;
        }

        public ItemInstance Equip(ItemLocation location, ItemEquipSlots equipSlot)
        {

            throw new NotImplementedException();

            //_character.EquippedItems.Add()
        }
        public ItemInstance Unequip(ItemEquipSlots equipSlot)
        {

            throw new NotImplementedException();
            //_character.EquippedItems.Remove()
        }
        /// <summary>
        /// Creates an unidentified instance of the base item specified by ID in the next open bag slot.
        /// </summary>
        /// <param name="baseId">The ID of the base item being spawned.</param>
        /// <returns>An new instance of the base item that is unidentified. Name will be "? <c>ItemType</c>"</returns>
        /// <exception cref="Necromancy.Server.Systems.Item.ItemException">Thrown when inventory is full or base ID does not exist.</exception>
        public ItemInstance SpawnUnidentifiedItem(int baseId)
        {
            throw new NotImplementedException();
        }
        public ItemInstance SpawnIdentifiedItem(int baseId)
        {
            throw new NotImplementedException();
        }
        public List<ItemInstance> SpawnItemInstances(ItemZoneType itemZoneType, int[] baseIds, ItemSpawnParams[] spawnParams)
        {
            if (_character.ItemManager.GetTotalFreeSpace(itemZoneType) < baseIds.Length) throw new ItemException(ItemExceptionType.InventoryFull);
            ItemLocation[] nextOpenLocations = _character.ItemManager.NextOpenSlots(itemZoneType, baseIds.Length);
            List<ItemInstance> itemInstances = _itemDao.InsertItemInstances(_character.Id, nextOpenLocations, baseIds, spawnParams);
            foreach (ItemInstance item in itemInstances)
            {
                _character.ItemManager.PutItem(item.Location, item);
            }
            return itemInstances;
        }
        public List<ItemInstance> GetIdentifiedItems(params long[] spawnIds)
        {
            throw new NotImplementedException();
        }
        public ItemInstance GetIdentifiedItem(ItemLocation location)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A list of items in your adventure bag, bag slot, royal bag, and avatar inventory.</returns>
        public List<ItemInstance> LoadOwnedInventoryItems()
        {
            List<ItemInstance> ownedItems = _itemDao.SelectOwnedInventoryItems(_character.Id);
            //load bags first
            foreach (ItemInstance item in ownedItems)
            {
                if (item.Location.ZoneType == ItemZoneType.BagSlot)
                {
                    ItemLocation location = item.Location; //only needed on load inventory because item's location is already populated and it is not in the manager
                    item.Location = ItemLocation.InvalidLocation; //only needed on load inventory because item's location is already populated and it is not in the manager
                    _character.ItemManager.PutItem(location, item);
                }
            }
            foreach (ItemInstance item in ownedItems)
            {
                
                if (item.Location.ZoneType != ItemZoneType.BagSlot) {
                    ItemLocation location = item.Location; //only needed on load inventory because item's location is already populated and it is not in the manager
                    item.Location = ItemLocation.InvalidLocation; //only needed on load inventory because item's location is already populated and it is not in the manager
                    _character.ItemManager.PutItem(location, item);
                }
            }
            return ownedItems;
        }
        public ItemInstance[] Sort(ItemZoneType zone, byte container, short[] fromSlots, short[] toSlots, short[] quantities)
        {
            ItemInstance[] sortedItems = new ItemInstance[fromSlots.Length];
            ulong[] instanceIds = new ulong[fromSlots.Length];
            ItemLocation[] itemLocations = new ItemLocation[fromSlots.Length];
            for (int i =0; i < fromSlots.Length; i++)
            {
                sortedItems[i] = _character.ItemManager.RemoveItem(new ItemLocation(zone, container, fromSlots[i]));
                instanceIds[i] = sortedItems[i].InstanceID;
            }
            for (int i = 0; i < fromSlots.Length; i++)
            {
                itemLocations[i] = new ItemLocation(zone, container, toSlots[i]);
                _character.ItemManager.PutItem(itemLocations[i], sortedItems[i]);
            }
            _itemDao.UpdateItemLocations(instanceIds, itemLocations);
            return sortedItems;
        }
        public long Drop(ItemLocation location, byte quantity)
        {
            throw new NotImplementedException();
        }
        public long Sell(ItemLocation location, byte quantity)
        {
            throw new NotImplementedException();
        }
        public List<ItemInstance> Move(ItemLocation from, ItemLocation to, byte quantity, out MoveType moveType)
        {
            List<ItemInstance> movedItems = new List<ItemInstance>();
            ItemInstance fromItem = _character.ItemManager.GetItem(from);
            bool hasToItem = _character.ItemManager.HasItem(to);
            ItemInstance toItem = _character.ItemManager.GetItem(to);

            //check possible errors. these should only occur if client is compromised
            if (fromItem is null || quantity == 0) throw new ItemException(ItemExceptionType.Generic);
            if (quantity > fromItem.Quantity) throw new ItemException(ItemExceptionType.Amount);
            if (quantity > 1 && hasToItem && toItem.BaseID != fromItem.BaseID) throw new ItemException(ItemExceptionType.BagLocation);
            
            moveType = MoveType.None;
            if (!hasToItem && quantity == fromItem.Quantity)
            {
                movedItems = MoveItemPlace(to, fromItem);
                moveType = MoveType.Place;
            }
            else if (!hasToItem && quantity < fromItem.Quantity)
            {
                movedItems = MoveItemPlaceQuantity(to, fromItem, quantity);
                moveType = MoveType.PlaceQuantity;
            }
            else if (hasToItem && quantity == fromItem.Quantity && (fromItem.BaseID != toItem.BaseID || fromItem.Quantity == fromItem.MaxStackSize))
            {
                movedItems = MoveItemSwap(from, to, fromItem, toItem);
                moveType = MoveType.Swap;
            } 
            else if (hasToItem && quantity < fromItem.Quantity && toItem.BaseID == fromItem.BaseID)
            {
                movedItems = MoveItemAddQuantity(fromItem, toItem, quantity);
                moveType = MoveType.AddQuantity;
            }
            else if (hasToItem && quantity == fromItem.Quantity && toItem.BaseID == fromItem.BaseID && quantity <= (toItem.MaxStackSize - toItem.Quantity))
            {
                movedItems = MoveItemAllQuantity(fromItem, toItem, quantity);
                moveType = MoveType.AllQuantity;
            }
            
            return movedItems;
        }

        /// <summary>
        /// Used when the there is no item already in the end location and the quantity moved is equal to the total quantity of items in the original location.        
        /// </summary>
        /// <param name="to">Move to this location.</param>
        /// <param name="fromItem">Move this item.</param>
        /// <returns>A list with a single element of the moved item.</returns>
        private List<ItemInstance> MoveItemPlace(ItemLocation to, ItemInstance fromItem)
        {
            List<ItemInstance> movedItem = new List<ItemInstance>();
            _character.ItemManager.PutItem(to, fromItem);
            movedItem.Add(fromItem);

            ulong[] instanceIds = new ulong[1];
            ItemLocation[] locs = new ItemLocation[1];
            instanceIds[0] = movedItem[0].InstanceID;
            locs[0] = movedItem[0].Location;
            _itemDao.UpdateItemLocations(instanceIds, locs);

            return movedItem;
        }
        /// <summary>
        ///  Used when the there is an item already in the end location,the quantity moved is equal to the total quantity<br/>
        ///  of items in the original location, and the item at the end location is a different base item or stacked full.
        /// </summary>
        /// <param name="from">Swap back to this location.</param>
        /// <param name="to">Move to this location.</param>
        /// <param name="fromItem">Move this item.</param>
        /// <param name="toItem">Swap this item.</param>
        /// <returns>A list with two elements containing the moved items.</returns>
        private List<ItemInstance> MoveItemSwap(ItemLocation from, ItemLocation to, ItemInstance fromItem,  ItemInstance toItem)
        {
            List<ItemInstance> movedItems = new List<ItemInstance>();
            _character.ItemManager.PutItem(to, fromItem);
            _character.ItemManager.PutItem(from, toItem);
            movedItems.Add(fromItem);
            movedItems.Add(toItem);

            ulong[] instanceIds = new ulong[movedItems.Count];
            ItemLocation[] locs = new ItemLocation[2];
            for (int i = 0; i < movedItems.Count; i++)
            {
                instanceIds[i] = movedItems[i].InstanceID;
                locs[i] = movedItems[i].Location;
            }
            _itemDao.UpdateItemLocations(instanceIds, locs);

            return movedItems;
        }

        /// <summary>
        /// Used when the there is no item already in the end location and the quantity moved is less than total quantity of items in the original location.
        /// </summary>
        /// <param name="to">Move to this location.</param>
        /// <param name="fromItem">Move a quantity from this item.</param>
        /// <returns>A list with two elements containing the original item with less quantity and a new instance with the remaining amount.</returns>
        private List<ItemInstance> MoveItemPlaceQuantity(ItemLocation to, ItemInstance fromItem, byte quantity)
        {
            fromItem.Quantity -= quantity;

            ItemSpawnParams spawnParam = new ItemSpawnParams();
            spawnParam.Quantity = quantity;
            spawnParam.ItemStatuses = fromItem.Statuses;

            const int size = 1;
            ItemLocation[] locs = new ItemLocation[size];
            int[] baseIds = new int[size];
            ItemSpawnParams[] spawnParams = new ItemSpawnParams[size];

            locs[0] = to;
            baseIds[0] = fromItem.BaseID;
            spawnParams[0] = spawnParam;

            List<ItemInstance> movedItems = _itemDao.InsertItemInstances(fromItem.OwnerID, locs, baseIds, spawnParams);
            _character.ItemManager.PutItem(to, movedItems[0]);
            movedItems.Insert(0, fromItem);

            return movedItems;
        }

        /// <summary>
        /// Used if there is the same item at the end location that is not at max stack size and the quantity moved is less than total quantity of items in the original location.<br/>
        /// If the stack would be filled with less than the passed quantity, fill the stack and return leftovers.
        /// </summary>        
        /// <param name="fromItem">Item to subract quantity from.</param>
        /// <param name="toItem">Location of item to add quantity to.</param>
        /// <param name="quantity">Maximum amount to transfer.</param>
        /// <returns>A list with two elements containing the original items with updated quantities.</returns>
        private List<ItemInstance> MoveItemAddQuantity(ItemInstance fromItem,ItemInstance toItem, byte quantity)
        {
            int freeSpace = toItem.MaxStackSize - toItem.Quantity;
            if (freeSpace < quantity)
                quantity = (byte) freeSpace;
            fromItem.Quantity -= quantity;
            toItem.Quantity += quantity;

            ulong[] instanceIds = new ulong[2];
            byte[] quantities = new byte[2];
            instanceIds[0] = fromItem.InstanceID;
            quantities[0] = fromItem.Quantity;
            instanceIds[1] = toItem.InstanceID;
            quantities[1] = toItem.Quantity;
            _itemDao.UpdateItemQuantities(instanceIds, quantities);

            List<ItemInstance> movedItems = new List<ItemInstance>(2);
            movedItems.Add(fromItem);
            movedItems.Add(toItem);
            return movedItems;
        }
        /// <summary>
        /// Used if there is the same item at the end location that is not at max stack size and the quantity moved is less equal to the quantity of items in the original location.        
        /// </summary>
        /// <param name="fromItem">Removed item.</param>
        /// <param name="toItem">Updated item.</param>
        /// <param name="quantity">Quantity to add to end item</param>
        /// <returns>A list with one elements containing the end item with an updated quantity.</returns>
        private List<ItemInstance> MoveItemAllQuantity(ItemInstance fromItem, ItemInstance toItem, byte quantity)
        {
            List<ItemInstance> movedItems = new List<ItemInstance>(1);
            toItem.Quantity += quantity;
            movedItems.Add(toItem);
            _character.ItemManager.RemoveItem(fromItem.Location);

            ulong[] instanceIds = new ulong[1];
            byte[] quantities = new byte[1];
            instanceIds[0] = toItem.InstanceID;
            quantities[0] = toItem.Quantity;
            //TODO MAKE TRANSACTION
            _itemDao.DeleteItemInstance(fromItem.InstanceID);
            _itemDao.UpdateItemQuantities(instanceIds, quantities);

            return movedItems;
        }

        public List<ItemInstance> Repair(List<ItemLocation> locations)
        {
            throw new NotImplementedException();
        }
        public long SubtractGold(long amount)
        {
            throw new NotImplementedException();
        }
        public long AddGold(long amount)
        {
            throw new NotImplementedException();
        }
    }
}
