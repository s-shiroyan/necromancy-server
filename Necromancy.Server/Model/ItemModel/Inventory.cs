using System.Collections.Generic;

namespace Necromancy.Server.Model.ItemModel
{
    public class Inventory
    {
        // TODO remove quick test, replace with BAG class
        private Dictionary<byte, InventoryItem[]> _inventory;
        private const int bagSize = 24;


        public Inventory()
        {
            _inventory = new Dictionary<byte, InventoryItem[]>();
            _inventory.Add(0, new InventoryItem[bagSize]);
        }

        public ItemActionResultType MoveInventoryItem(InventoryItem inventoryItem, byte bagId, short bagSlotIndex)
        {
            return ItemActionResultType.Ok;
        }

        public InventoryItem GetEquippedInventoryItem(EquipmentSlotType equipmentSlotType)
        {
            foreach (InventoryItem[] bag in _inventory.Values)
            {
                for (int i = 0; i < bag.Length; i++)
                {
                    InventoryItem inventoryItem = bag[i];
                    if (inventoryItem != null && inventoryItem.CurrentEquipmentSlotType == equipmentSlotType)
                    {
                        return inventoryItem;
                    }
                }
            }

            return null;
        }

        public InventoryItem GetInventoryItem(int inventoryItemId)
        {
            foreach (InventoryItem[] bag in _inventory.Values)
            {
                for (int i = 0; i < bag.Length; i++)
                {
                    InventoryItem inventoryItem = bag[i];
                    if (inventoryItem != null && inventoryItem.Id == inventoryItemId)
                    {
                        return inventoryItem;
                    }
                }
            }

            return null;
        }

        public InventoryItem GetInventoryItem(byte bagId, short bagSlotIndex)
        {
            if (_inventory.ContainsKey(bagId))
            {
                InventoryItem[] bag = _inventory[bagId];
                if (bag.Length >= bagSlotIndex)
                {
                    return bag[bagSlotIndex];
                }
            }

            return null;
        }

        public bool RemoveInventoryItem(InventoryItem inventoryItem)
        {
            if (_inventory.ContainsKey(inventoryItem.BagId))
            {
                InventoryItem[] bag = _inventory[inventoryItem.BagId];
                if (bag[inventoryItem.BagSlotIndex] == inventoryItem)
                {
                    bag[inventoryItem.BagSlotIndex] = null;
                    return true;
                }
            }

            return false;
        }

        public bool AddInventoryItem(InventoryItem inventoryItem)
        {
            foreach (byte bagId in _inventory.Keys)
            {
                InventoryItem[] bag = _inventory[bagId];
                for (short bagSlotIndex = 0; bagSlotIndex < bag.Length; bagSlotIndex++)
                {
                    if (bag[bagSlotIndex] == null)
                    {
                        bag[bagSlotIndex] = inventoryItem;
                        inventoryItem.BagId = bagId;
                        inventoryItem.BagSlotIndex = bagSlotIndex;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool AddInventoryItem(InventoryItem inventoryItem, byte bagId, short bagSlotIndex)
        {
            inventoryItem.BagId = bagId;
            inventoryItem.BagSlotIndex = bagSlotIndex;
            if (_inventory.ContainsKey(bagId))
            {
                InventoryItem[] bag = _inventory[bagId];
                bag[bagSlotIndex] = inventoryItem;
            }
            else
            {
                InventoryItem[] bag = new InventoryItem[bagSize];
                bag[bagSlotIndex] = inventoryItem;
                _inventory.Add(bagId, bag);
            }

            return true;
        }
    }
}
