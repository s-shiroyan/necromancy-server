using Arrowgene.Logging;
using System.Collections.Generic;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Model.ItemModel
{
    public class Inventory
    {
        // TODO remove quick test, replace with BAG class
        private Dictionary<byte, Dictionary<byte, InventoryItem[]>> _storageContainers;
        public Dictionary<byte, InventoryItem[]> _inventory;
        public Dictionary<byte, InventoryItem[]> _royalBag;
        public Dictionary<EquipmentSlotType, InventoryItem> _equippedItems;

        private Dictionary<byte, InventoryItem[]> _cloakRoom;
        private Dictionary<byte, InventoryItem[]> _avatar;
        private Dictionary<byte, InventoryItem[]> _unionStorage;
        private Dictionary<byte, InventoryItem[]> _treasureBox;


        private const int bagSize = 24; //temporary
        private const int cloakRoomTabSize = 32;
        private const int avatarTabSize = 50;
        private const int unionTabSize = 50;
        private const int treasureBoxSize = 10;

        private Logger logger;
        public Inventory()
        {
            _inventory = new Dictionary<byte, InventoryItem[]>();
            _inventory.Add(0, new InventoryItem[bagSize]);
            _royalBag = new Dictionary<byte, InventoryItem[]>();
            _royalBag.Add(0, new InventoryItem[bagSize]);

            _equippedItems = new Dictionary<EquipmentSlotType, InventoryItem>();
            _cloakRoom = new Dictionary<byte, InventoryItem[]>();
            _cloakRoom.Add(0, new InventoryItem[cloakRoomTabSize]);
            _avatar = new Dictionary<byte, InventoryItem[]>();
            _avatar.Add(0, new InventoryItem[avatarTabSize]);
            _avatar.Add(1, new InventoryItem[avatarTabSize]);
            _avatar.Add(2, new InventoryItem[avatarTabSize]);
            _unionStorage = new Dictionary<byte, InventoryItem[]>();
            _unionStorage.Add(0, new InventoryItem[unionTabSize]);
            _treasureBox = new Dictionary<byte, InventoryItem[]>();
            _treasureBox.Add(0, new InventoryItem[treasureBoxSize]);

            _storageContainers = new Dictionary<byte, Dictionary<byte, InventoryItem[]>>();
            _storageContainers.Add(0, _inventory);
            _storageContainers.Add(2, _royalBag); 
            _storageContainers.Add(3, _cloakRoom);
            _storageContainers.Add(12, _avatar);
            _storageContainers.Add(0x51, _unionStorage);
            _storageContainers.Add(66, _treasureBox);

        }
        public void Equip(InventoryItem inventoryItem)
        {
            if (_equippedItems.ContainsKey(inventoryItem.Item.EquipmentSlotType))
            {
                UnEquip(inventoryItem);
            }
            _equippedItems.Add(inventoryItem.Item.EquipmentSlotType, inventoryItem);
        }
        public void UnEquip(InventoryItem inventoryItem)
        {
            _equippedItems.Remove(inventoryItem.Item.EquipmentSlotType);
        }
        public InventoryItem CheckAlreadyEquipped(EquipmentSlotType equipmentSlotType)
        {
            InventoryItem inventoryItem = null;
            if (equipmentSlotType == EquipmentSlotType.HAND_R | equipmentSlotType == EquipmentSlotType.HAND_L)
            {
                if (_equippedItems.ContainsKey(EquipmentSlotType.HAND_L | EquipmentSlotType.HAND_R))
                {
                    _equippedItems.TryGetValue((EquipmentSlotType.HAND_L | EquipmentSlotType.HAND_R), out inventoryItem);
                }
                else
                {
                    _equippedItems.TryGetValue(equipmentSlotType, out inventoryItem);
                }
            }
            else
            {
                _equippedItems.TryGetValue(equipmentSlotType, out inventoryItem);
            }
            return inventoryItem;
        }

        public ItemActionResultType MoveInventoryItem(InventoryItem inventoryItem, byte storageType, byte bagId, short bagSlotIndex)
        {
            if (_storageContainers.ContainsKey(storageType))
            {
                _storageContainers.TryGetValue(storageType, out Dictionary<byte, InventoryItem[]> _storageContainer);
                _storageContainers.TryGetValue(inventoryItem.StorageType, out Dictionary<byte, InventoryItem[]> _storageContainerFrom);
                if (_storageContainerFrom.ContainsKey(inventoryItem.BagId))
                {
                    InventoryItem[] bag = _storageContainerFrom[inventoryItem.BagId];
                    if (bag[inventoryItem.BagSlotIndex] == inventoryItem)
                    {
                        bag[inventoryItem.BagSlotIndex] = null;
                    }
                }
                else
                {
                    logger.Error("missing bag. this is not possible.");
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
                    if (inventoryItem != null && (inventoryItem.CurrentEquipmentSlotType.HasFlag(equipmentSlotType)))
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


        public bool AddAvatarItem(InventoryItem inventoryItem)
        {
            foreach (byte bagId in _avatar.Keys)
            {
                InventoryItem[] bag = _avatar[bagId];
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

        public bool AddTreasureBoxItem(InventoryItem inventoryItem)
        {
            foreach (byte bagId in _treasureBox.Keys)
            {
                InventoryItem[] bag = _treasureBox[bagId];
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

        public void LoginLoadInventory(InventoryItem inventoryItem)
        {
            if (_storageContainers.ContainsKey(inventoryItem.StorageType))
            {
                _storageContainers.TryGetValue(inventoryItem.StorageType, out Dictionary<byte, InventoryItem[]> _storageContainer);

                if (_storageContainer.ContainsKey(inventoryItem.BagId))
                {
                    InventoryItem[] bag = _storageContainer[inventoryItem.BagId];
                    bag[inventoryItem.BagSlotIndex] = inventoryItem;
                }

            }
        }

        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(Inventory));
        public void LogEquppedItems()
        {
            foreach (InventoryItem[] bag in _inventory.Values)
            {
                for (int i = 0; i < bag.Length; i++)
                {
                    InventoryItem inventoryItem = bag[i];
                    if (inventoryItem != null && inventoryItem.CurrentEquipmentSlotType != EquipmentSlotType.NONE)
                    {
                        Logger.Debug($"Equipped: {inventoryItem.Id} {inventoryItem.Item.Name} -> {inventoryItem.CurrentEquipmentSlotType}");
                    }
                }
            }
        }
    }
}
