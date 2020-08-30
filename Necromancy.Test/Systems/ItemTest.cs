
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Items;
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

        public ItemTest()
        {
            _dummyCharacter = new Character();
            _itemService = new ItemService(_dummyCharacter);
        }

        [Fact]
        public void TestSpawnUnidentified()
        {
            const int VALID_BASE_ID = 5;
            SpawnedItem newUnidentifiedItem;

            newUnidentifiedItem = _itemService.SpawnUnidentifiedItem(VALID_BASE_ID);

            Assert.Equal(VALID_BASE_ID, newUnidentifiedItem.BaseId);
        }

        [Fact]
        public void TestSpawnUnidentifiedItemNoItemFound()
        {
            const int INVALID_BASE_ID = 5;            
            ItemException e = Assert.Throws<ItemException>(() => _itemService.SpawnUnidentifiedItem(INVALID_BASE_ID));
            Assert.Equal(ItemExceptionType.Generic, e.ExceptionType);
        }

        [Fact]
        public void TestGetSpawnItemNoSpawnFound()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestGetBaseItem()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestGetBaseItemNoItemFound()
        {
            throw new NotImplementedException();
        }
    }
}
