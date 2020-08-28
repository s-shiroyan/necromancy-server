using Necromancy.Server.Model;
using System;
using System.Collections.Generic;

namespace Necromancy.Server.Systems.Items
{
    public class ItemService
    {
        private readonly Character _character;
        public ItemService(Character character)
        {
            _character = character;
        }
        public SpawnedItem Equip(ItemLocation location, ItemEquipSlot equipSlot)
        {
            //_character.EquippedItems.Add()
            throw new NotImplementedException();
        }
        public SpawnedItem Unequip(ItemEquipSlot equipSlot)
        {
            //_character.EquippedItems.Remove()
            throw new NotImplementedException();
        }
        /// <summary>
        /// Creates an unidentified instance of the base item specified by ID in the next open bag slot.
        /// </summary>
        /// <param name="baseId">The ID of the base item being spawned.</param>
        /// <returns>An new instance of the base item that is unidentified. Name will be "? <c>ItemType</c>"</returns>
        /// <exception cref="Necromancy.Server.Systems.Items.ItemException">Thrown when inventory is full or base ID does not exist.</exception>
        public SpawnedItem SpawnUnidentifiedItem(int baseId)
        {
            throw new NotImplementedException();
        }
        public SpawnedItem SpawnIdentifiedItem(int baseId)
        {
            throw new NotImplementedException();
        }
        public List<SpawnedItem> GetIdentifiedItems(params long[] spawnIds)
        {            
            throw new NotImplementedException();
        }
        public SpawnedItem GetIdentifiedItem(ItemLocation location)
        {
            throw new NotImplementedException();
        }
        public List<SpawnedItem> GetOwnedItems()
        {
            throw new NotImplementedException();
        }
        public long Drop(ItemLocation location, byte quantity)
        {
            throw new NotImplementedException();
        }
        public long Sell(ItemLocation location, byte quantity)
        {
            throw new NotImplementedException();
        }
        public List<SpawnedItem> Move(ItemLocation from, ItemLocation to, byte quantity)
        {
            throw new NotImplementedException();
        }
        public List<SpawnedItem> Repair(List<ItemLocation> locations)
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
