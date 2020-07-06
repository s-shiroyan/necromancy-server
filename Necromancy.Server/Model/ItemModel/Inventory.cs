using System.Collections.Generic;

namespace Necromancy.Server.Model.ItemModel
{
    public class Inventory
    {
        // TODO remove quick test, replace with BAG class
        private Dictionary<byte, InventoryItem[]> _inventory;
        private const int bagSize = 24;
        private const int cloakRoomSize = 30;



        public Inventory()
        {
            _inventory = new Dictionary<byte, InventoryItem[]>();
            _inventory.Add(0, new InventoryItem[bagSize]);
            _inventory.Add(1, new InventoryItem[bagSize]);
            _inventory.Add(2, new InventoryItem[bagSize]);
            _inventory.Add(3, new InventoryItem[cloakRoomSize]);
            _inventory.Add(4, new InventoryItem[bagSize]);
        }

        public ItemActionResultType MoveInventoryItem(InventoryItem inventoryItem, byte storageType, byte bagId, short bagSlotIndex)
        {
            if (_inventory.ContainsKey(inventoryItem.BagId))
            {
                InventoryItem[] bag = _inventory[inventoryItem.BagId];
                if (bag[inventoryItem.BagSlotIndex] == inventoryItem)
                {
                    bag[inventoryItem.BagSlotIndex] = null;
                }
            }
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
            return ItemActionResultType.Ok;
        }

        public ItemActionResultType SwapInventoryItem(InventoryItem inventoryItem, byte storageType, byte bagId, short bagSlotIndex, InventoryItem inventoryItem2, byte storageType2, byte bagId2, short bagSlotIndex2)
        {
            if (_inventory.ContainsKey(inventoryItem.BagId))
            {
                InventoryItem[] bag = _inventory[inventoryItem.BagId];
                if (bag[inventoryItem.BagSlotIndex] == inventoryItem)
                {
                    bag[inventoryItem.BagSlotIndex] = inventoryItem2;
                }
            }
            inventoryItem.StorageType = storageType;
            inventoryItem.BagId = bagId;
            inventoryItem.BagSlotIndex = bagSlotIndex;

            if (_inventory.ContainsKey(inventoryItem2.BagId))
            {
                InventoryItem[] bag = _inventory[inventoryItem2.BagId];
                if (bag[inventoryItem2.BagSlotIndex] == inventoryItem2)
                {
                    bag[inventoryItem2.BagSlotIndex] = inventoryItem;
                }
            }
            inventoryItem2.StorageType = storageType2;
            inventoryItem2.BagId = bagId2;
            inventoryItem2.BagSlotIndex = bagSlotIndex2;
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
