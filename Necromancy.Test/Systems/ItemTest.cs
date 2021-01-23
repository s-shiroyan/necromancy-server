
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Item;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace Necromancy.Test.Systems
{
    public class ItemTest
    {
        private readonly Character _dummyCharacter;
        private readonly ItemService _itemService;

        private class DummyItemDao : IItemDao
        {
            public void DeleteItemInstance(ulong instanceIds)
            {
                //ignore
            }

            public ItemInstance InsertItemInstance(int baseId)
            {
                throw new NotImplementedException();
            }

            public List<ItemInstance> InsertItemInstances(int ownerId, ItemLocation[] locs, int[] baseId, ItemSpawnParams[] spawnParams)
            {
                List<ItemInstance> dummyItems = new List<ItemInstance>(2);
                for(int i = 0; i < locs.Length; i++)
                {
                    ItemInstance itemInstance = new ItemInstance((ulong)i);
                    itemInstance.BaseID = baseId[i];
                    itemInstance.Location = locs[i];
                    itemInstance.Quantity = spawnParams[i].Quantity;
                    dummyItems.Add(itemInstance);
                }
                return dummyItems;
            }

            public ItemInstance SelectItemInstance(long instanceId)
            {
                throw new NotImplementedException();
            }

            public ItemInstance SelectItemInstance(int characterId, ItemLocation itemLocation)
            {
                throw new NotImplementedException();
            }

            public List<ItemInstance> SelectOwnedInventoryItems(int ownerId)
            {
                throw new NotImplementedException();
            }

            public void UpdateItemLocations(ulong[] instanceIds, ItemLocation[] locs)
            {
                //ignore
            }

            public void UpdateItemQuantities(ulong[] instanceIds, byte[] quantities)
            {
                //ignore
            }
        }

        public ItemTest()
        {
            _dummyCharacter = new Character();
            _itemService = new ItemService(_dummyCharacter, new DummyItemDao());
        }

        [Fact(Skip = "NYI")]
        public void TestSpawnUnidentified()
        {
            const int VALID_BASE_ID = 5;
            ItemInstance newUnidentifiedItem;

            newUnidentifiedItem = _itemService.SpawnUnidentifiedItem(VALID_BASE_ID);

            Assert.Equal(VALID_BASE_ID, newUnidentifiedItem.BaseID);
        }

        [Fact(Skip = "NYI")]
        public void TestSpawnUnidentifiedItemNoItemFound()
        {
            const int INVALID_BASE_ID = 5;            
            ItemException e = Assert.Throws<ItemException>(() => _itemService.SpawnUnidentifiedItem(INVALID_BASE_ID));
            Assert.Equal(ItemExceptionType.Generic, e.ExceptionType);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public void TestItemMovePlace(byte quantity)
        {
            const ulong instanceId = 756366;
            ItemInstance itemInstance = new ItemInstance(instanceId);
            itemInstance.Quantity = quantity;
            ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
            _dummyCharacter.ItemManager.PutItem(fromLoc,itemInstance);
            ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

            _itemService.Move(fromLoc, toLoc, quantity, out ItemService.MoveType moveType);

            Assert.Equal(ItemService.MoveType.Place, moveType);
            Assert.Null(_dummyCharacter.ItemManager.GetItem(fromLoc));
            Assert.Equal(instanceId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
            Assert.Equal(itemInstance.Location, toLoc);
            Assert.Equal(quantity, itemInstance.Quantity);
        }

        [Fact]
        public void TestItemMovePlaceQuantity()
        {
            const ulong instanceId = 756366;
            const int startQuantity = 10;
            const int moveQuantity = 6;
            ItemInstance itemOriginal = new ItemInstance(instanceId);
            itemOriginal.Quantity = startQuantity;
            ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
            _dummyCharacter.ItemManager.PutItem(fromLoc, itemOriginal);
            ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

            List<ItemInstance> movedItems = _itemService.Move(fromLoc, toLoc, moveQuantity, out ItemService.MoveType moveType);

            Assert.Equal(ItemService.MoveType.PlaceQuantity, moveType);

            Assert.Equal(2, movedItems.Count);
            Assert.Equal(fromLoc, itemOriginal.Location);
            Assert.Equal(startQuantity - moveQuantity, itemOriginal.Quantity);
            Assert.NotNull(_dummyCharacter.ItemManager.GetItem(fromLoc));
            Assert.Equal(instanceId, _dummyCharacter.ItemManager.GetItem(fromLoc).InstanceID);

            Assert.Equal(itemOriginal.BaseID, movedItems[1].BaseID);
            Assert.Equal(toLoc, movedItems[1].Location);
            Assert.Equal(moveQuantity, movedItems[1].Quantity);
            Assert.NotNull(_dummyCharacter.ItemManager.GetItem(toLoc));
        }

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,1)]
        [InlineData(100,50)]
        [InlineData(50,67)]
        public void TestItemMoveSwap(byte fromQuantity, byte toQuantity)
        {
            const ulong fromId = 234987915;
            const ulong toId = 33388888;

            ItemInstance fromItem = new ItemInstance(fromId);
            fromItem.Quantity = fromQuantity;
            fromItem.MaxStackSize = fromQuantity;
            ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
            _dummyCharacter.ItemManager.PutItem(fromLoc, fromItem);

            ItemInstance toItem = new ItemInstance(toId);
            toItem.Quantity = toQuantity;
            ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
            _dummyCharacter.ItemManager.PutItem(toLoc, toItem);

            _itemService.Move(fromLoc, toLoc, fromQuantity, out ItemService.MoveType moveType);

            Assert.Equal(ItemService.MoveType.Swap, moveType);
            Assert.Equal(fromItem.Location, toLoc);
            Assert.Equal(fromId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
            Assert.Equal(fromQuantity, fromItem.Quantity);
            Assert.Equal(toItem.Location, fromLoc);
            Assert.Equal(toId, _dummyCharacter.ItemManager.GetItem(fromLoc).InstanceID);
            Assert.Equal(toQuantity, toItem.Quantity);
        }

        [Fact]
        public void TestItemMoveAddQuantity()
        {
            const ulong toId = 234987915;
            const ulong fromId = 34151555;
            const int fromQuantity = 10;
            const int toQuantity = 30;
            const int moveQuantity = fromQuantity - 1;

            ItemInstance fromItem = new ItemInstance(fromId);
            fromItem.Quantity = fromQuantity;
            ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
            _dummyCharacter.ItemManager.PutItem(fromLoc, fromItem);

            ItemInstance toItem = new ItemInstance(toId);
            toItem.MaxStackSize = toQuantity + moveQuantity + 5;
            toItem.Quantity = toQuantity;
            ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
            _dummyCharacter.ItemManager.PutItem(toLoc, toItem);

            _itemService.Move(fromLoc, toLoc, moveQuantity, out ItemService.MoveType moveType);

            Assert.Equal(ItemService.MoveType.AddQuantity, moveType);
            Assert.Equal(fromId, _dummyCharacter.ItemManager.GetItem(fromLoc).InstanceID);
            Assert.Equal(fromQuantity - moveQuantity, fromItem.Quantity);
            Assert.Equal(toId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
            Assert.Equal(toQuantity + moveQuantity, toItem.Quantity);
        }

        [Fact]
        public void TestItemMovePlaceAllQuantity()
        {
            const ulong fromId = 34151555;
            const ulong toId = 234987915;
            const int fromQuantity = 10;
            const int toQuantity = 30;

            ItemInstance fromItem = new ItemInstance(fromId);
            fromItem.Quantity = fromQuantity;
            ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
            _dummyCharacter.ItemManager.PutItem(fromLoc, fromItem);

            ItemInstance toItem = new ItemInstance(toId);
            toItem.MaxStackSize = toQuantity + fromQuantity + 5;
            toItem.Quantity = toQuantity;
            ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
            _dummyCharacter.ItemManager.PutItem(toLoc, toItem);

            _itemService.Move(fromLoc, toLoc, fromQuantity, out ItemService.MoveType moveType);

            Assert.Equal(ItemService.MoveType.AllQuantity, moveType);
            Assert.Null(_dummyCharacter.ItemManager.GetItem(fromLoc));
            Assert.Equal(fromQuantity + toQuantity, toItem.Quantity);
            Assert.Equal(toId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
        }

        [Fact]
        public void TestItemMovePlaceEquippedBag()
        {
            const ulong instanceId = 756366;
            const ulong bagId = 534577777;
            const int quantity = 1;
            ItemInstance itemInstance = new ItemInstance(instanceId);            
            ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
            _dummyCharacter.ItemManager.PutItem(fromLoc, itemInstance);

            ItemInstance equippedBag = new ItemInstance(bagId);
            equippedBag.BagSize = 10;
            ItemLocation bagLocation = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
            _dummyCharacter.ItemManager.PutItem(bagLocation, equippedBag);
            ItemLocation toLoc = new ItemLocation(ItemZoneType.EquippedBags, 0, 1);

            _itemService.Move(fromLoc, toLoc, quantity, out ItemService.MoveType moveType);

            Assert.Equal(ItemService.MoveType.Place, moveType);
            Assert.Null(_dummyCharacter.ItemManager.GetItem(fromLoc));
            Assert.Equal(instanceId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
            Assert.Equal(itemInstance.Location, toLoc);
            Assert.Equal(quantity, itemInstance.Quantity);
        }

        [Fact]
        public void TestItemMoveEmptyBagOutOfSlot()
        {
            const ulong bagId = 534577777;
            const int quantity = 1;
            ItemInstance bag = new ItemInstance(bagId);
            ItemLocation bagLoc = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
            _dummyCharacter.ItemManager.PutItem(bagLoc, bag);
            ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

            _itemService.Move(bagLoc, toLoc, quantity, out ItemService.MoveType moveType);

            Assert.Equal(ItemService.MoveType.Place, moveType);
            Assert.Null(_dummyCharacter.ItemManager.GetItem(bagLoc));
            Assert.Equal(bagId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
            Assert.Equal(bag.Location, toLoc);
            Assert.Equal(quantity, bag.Quantity);
        }
    }
}
