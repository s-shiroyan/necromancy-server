using Arrowgene.Logging;
using System.Collections.Generic;

namespace Necromancy.Server.Model.ItemModel
{
    public class Inventory
    {
        // TODO remove quick test, replace with BAG class
        private Dictionary<byte, Dictionary<byte, InventoryItem[]>> _storageContainers;
        private Dictionary<byte, InventoryItem[]> _inventory;
        private Dictionary<byte, InventoryItem[]> _cloakRoom;
        private Dictionary<byte, InventoryItem[]> _avatar;
        private Dictionary<byte, InventoryItem[]> _unionStorage;

        private const int bagSize = 24; //temporary
        private const int cloakRoomTabSize = 30;
        private const int avatarTabSize = 50;

        private Logger logger;



        public Inventory()
        {
            _inventory = new Dictionary<byte, InventoryItem[]>();
            _inventory.Add(0, new InventoryItem[bagSize]);
            _cloakRoom = new Dictionary<byte, InventoryItem[]>();
            _cloakRoom.Add(0, new InventoryItem[cloakRoomTabSize]);
            _avatar = new Dictionary<byte, InventoryItem[]>();
            _avatar.Add(0, new InventoryItem[avatarTabSize]);
            _avatar.Add(1, new InventoryItem[avatarTabSize]);
            _avatar.Add(2, new InventoryItem[avatarTabSize]);
            _unionStorage = new Dictionary<byte, InventoryItem[]>();
            _unionStorage.Add(0, new InventoryItem[bagSize]);

            _storageContainers = new Dictionary<byte, Dictionary<byte, InventoryItem[]>>();
            _storageContainers.Add(0, _inventory);
            _storageContainers.Add(3, _cloakRoom);
            _storageContainers.Add(4, _avatar);
            _storageContainers.Add(5, _unionStorage);

        }

        public ItemActionResultType MoveInventoryItem(InventoryItem inventoryItem, byte storageType, byte bagId, short bagSlotIndex)
        {
            if (_storageContainers.ContainsKey(storageType))
            {
                _storageContainers.TryGetValue(storageType, out Dictionary<byte, InventoryItem[]> _storageContainer);
                if (_storageContainer.ContainsKey(bagId))
                {
                    InventoryItem[] bag = _storageContainer[inventoryItem.BagId];
                    if (bag[inventoryItem.BagSlotIndex] == inventoryItem)
                    {
                        bag[inventoryItem.BagSlotIndex] = null;
                    }
                }
                inventoryItem.StorageType = storageType;
                inventoryItem.BagId = bagId;
                inventoryItem.BagSlotIndex = bagSlotIndex;
                if (_storageContainer.ContainsKey(bagId))
                {
                    InventoryItem[] bag = _storageContainer[bagId];
                    bag[bagSlotIndex] = inventoryItem;
                }
                else
                {
                    logger.Error("had to create a bag. this is not intended.");
                    InventoryItem[] bag = new InventoryItem[bagSize];
                    bag[bagSlotIndex] = inventoryItem;
                    _storageContainer.Add(bagId, bag);
                }
                return ItemActionResultType.Ok;

            }
            else
            {
                return ItemActionResultType.ErrorBagLocation;
            }

        }

        public ItemActionResultType SwapInventoryItem(InventoryItem inventoryItem, byte storageType, byte bagId, short bagSlotIndex, InventoryItem inventoryItem2, byte storageType2, byte bagId2, short bagSlotIndex2)
        {
            if (_storageContainers.ContainsKey(storageType) && _storageContainers.ContainsKey(storageType2))
            {
                _storageContainers.TryGetValue(storageType, out Dictionary<byte, InventoryItem[]> _storageContainer);
                _storageContainers.TryGetValue(storageType2, out Dictionary<byte, InventoryItem[]> _storageContainer2);

                if (_storageContainer.ContainsKey(bagId))
                {
                    InventoryItem[] bag = _storageContainer[inventoryItem.BagId];
                    //if (bag[inventoryItem.BagSlotIndex] == inventoryItem)
                    {
                        bag[inventoryItem.BagSlotIndex] = inventoryItem2;
                    }
                }
                inventoryItem.StorageType = storageType;
                inventoryItem.BagId = bagId;
                inventoryItem.BagSlotIndex = bagSlotIndex;

                if (_storageContainer2.ContainsKey(inventoryItem2.BagId))
                {
                    InventoryItem[] bag = _storageContainer2[inventoryItem2.BagId];
                    //if (bag[inventoryItem2.BagSlotIndex] == inventoryItem2)
                    {
                        bag[inventoryItem2.BagSlotIndex] = inventoryItem;
                    }
                }
                inventoryItem2.StorageType = storageType2;
                inventoryItem2.BagId = bagId2;
                inventoryItem2.BagSlotIndex = bagSlotIndex2;
                return ItemActionResultType.Ok;
            }
            else
            {
                return ItemActionResultType.ErrorBagLocation;
            }

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
        public InventoryItem GetInventoryItem(byte storageType, byte bagId, short bagSlotIndex)
        {
            if (_storageContainers.ContainsKey(storageType))
            {
                _storageContainers.TryGetValue(storageType, out Dictionary<byte, InventoryItem[]> _storageContainer);

                if (_storageContainer.ContainsKey(bagId))
                {
                    InventoryItem[] bag = _storageContainer[bagId];
                    if (bag.Length >= bagSlotIndex)
                    {
                        return bag[bagSlotIndex];
                    }
                }
            }
            logger.Error("null hit on GetInventoryItem");
            return null;
        }

        public bool RemoveInventoryItem(InventoryItem inventoryItem)
        {
            if (_storageContainers.ContainsKey(inventoryItem.StorageType))
            {
                _storageContainers.TryGetValue(inventoryItem.StorageType, out Dictionary<byte, InventoryItem[]> _storageContainer);

                if (_storageContainer.ContainsKey(inventoryItem.BagId))
                {
                    InventoryItem[] bag = _storageContainer[inventoryItem.BagId];
                    if (bag[inventoryItem.BagSlotIndex] == inventoryItem)
                    {
                        bag[inventoryItem.BagSlotIndex] = null;
                        return true;
                    }
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
