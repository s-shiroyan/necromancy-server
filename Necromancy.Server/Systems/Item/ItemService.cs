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
            foreach(ItemInstance item in itemInstances)
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
        public void LoadOwnedInventoryItems()
        {
            List<ItemInstance> ownedItems = _itemDao.SelectOwnedInventoryItems(_character.Id);
            //load bags first
            foreach(ItemInstance item in ownedItems)
            {
                if (item.Type == ItemType.BAG) _character.ItemManager.PutItem(item.Location, item);
            }
            foreach (ItemInstance item in ownedItems)
            {
                if (item.Type != ItemType.BAG) _character.ItemManager.PutItem(item.Location, item);
            }
        }
        public void Sort(ItemZoneType zone, byte container)
        {

        }
        public long Drop(ItemLocation location, byte quantity)
        {
            throw new NotImplementedException();
        }
        public long Sell(ItemLocation location, byte quantity)
        {
            throw new NotImplementedException();
        }
        public List<ItemInstance> Move(ItemLocation from, ItemLocation to, byte quantity)
        {
            throw new NotImplementedException();
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
