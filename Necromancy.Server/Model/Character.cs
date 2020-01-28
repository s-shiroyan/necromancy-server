using System.Linq;
using System;
using System.Collections.Generic;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Packet.Receive;

namespace Necromancy.Server.Model
{
    public class Character : IInstance
    {
        private readonly object StateLock = new object();
        public uint InstanceId { get; set; }

        //core attributes
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SoulId { get; set; }
        public DateTime Created { get; set; }
        public byte Slot { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
        public uint Raceid { get; set; }
        public uint Sexid { get; set; }
        public byte HairId { get; set; }
        public byte HairColorId { get; set; }
        public byte FaceId { get; set; }
        public uint Alignmentid { get; set; }
        public ushort Strength { get; set; }
        public ushort vitality { get; set; }
        public ushort dexterity { get; set; }
        public ushort agility { get; set; }
        public ushort intelligence { get; set; }
        public ushort piety { get; set; }
        public ushort luck { get; set; }
        public uint ClassId { get; set; }
        public int maxHp { get; set; }
        public uint maxMp { get; set; }
        public uint maxOd { get; set; }
        public bool hadDied { get; set; }
        public uint DeadBodyInstanceId { get; set; }
        public int Channel { get; set; }
        public int beginnerProtection { get; set; }
        public uint state { get; set; }

        //Movement Related
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public byte battlePose { get; set; }
        public byte battleAnim { get; set; }
        public byte battleNext { get; set; }
        public int charaPose { get; set; }
        public byte movementPose { get; set; }
        public byte movementAnim { get; set; }
        public bool weaponEquipped { get; set; }
        public uint movementId { get; set; }

        //Normal Attack
        public int[] AttackIds { get; set; }
        //Logout Cancel Detection
        public byte logoutCanceled { get; set; }

        //Map Related
        public int MapId { get; set; }

        //Temporary Value Holders
        public int WeaponType { get; set; }
        public int AdventureBagGold { get; set; }
        public byte soulFormState { get; set; }
        public int[] EquipId { get; set; }
        public int eventSelectExecCode { get; set; }
        public uint activeSkillInstance { get; set; }
        public bool castingSkill { get; set; }
        public int nextBagSlot { get; set; } // Until bag management is done
        public uint eventSelectReadyCode { get; set; }
        public int currentHp { get; set; }
        public uint currentMp { get; set; }
        public uint currentOd { get; set; }
        public int shortcutBar0Id { get; set; }
        public int shortcutBar1Id { get; set; }
        public int shortcutBar2Id { get; set; }
        public int shortcutBar3Id { get; set; }
        public int shortcutBar4Id { get; set; }
        public List<InventoryItem> inventoryItems { get; set; }
        public List<Bag> inventoryBags { get; set; }
        public bool takeover { get; set; }
        public int skillStartCast { get; set; }
        public bool helperText { get; set; }
        public bool helperTextBlacksmith { get; set; }
        public bool helperTextDonkey { get; set; }
        public bool helperTextCloakRoom { get; set; }
        public Event currentEvent { get; set; }
        public bool secondInnAccess { get; set; }
        public uint partyId { get; set; }

        //Msg Value Holders
        public uint friendRequest { get; set; }
        public uint partyRequest { get; set; }


        public Character()
        {
            Id = -1;
            AccountId = -1;
            SoulId = -1;
            Created = DateTime.Now;
            MapId = -1;
            X = 0;
            Y = 0;
            Z = 0;
            Slot = 0;
            Name = null;
            Level = 0;
            ////// 
            logoutCanceled = 0;
            WeaponType = 8;
            AdventureBagGold = 80706050;
            eventSelectExecCode = -1;
            maxHp = 1000;
            maxMp = 500;
            maxOd = 200;
            currentHp = 1000;
            currentMp = 450;
            currentOd = 150;
            shortcutBar0Id = -1;
            shortcutBar1Id = -1;
            shortcutBar2Id = -1;
            shortcutBar3Id = -1;
            shortcutBar4Id = -1;
            takeover = false;
            skillStartCast = 0;
            battleAnim = 0;
            hadDied = false;
            nextBagSlot = 0;
            state = 0b00000000;
            inventoryItems = new List<InventoryItem>();
            inventoryBags = new List<Bag>();
            Bag bag = new Bag();
            bag.StorageId = 0;
            bag.NumSlots = 24;
            inventoryBags.Add(bag);
            helperText = true;
            helperTextBlacksmith = true;
            helperTextDonkey = true;
            helperTextCloakRoom = true;
            beginnerProtection = 1;
            currentEvent = null;
           secondInnAccess = false;
            partyId = 0;
        }

        public uint GetState ()
        {
            uint charState = 0;
            lock (StateLock)
            {
                charState = state;
            }
            return charState;
        }
        public void SetState(uint charState)
        {
            lock (StateLock)
            {
                state = charState;
            }
        }
        public uint AddStateBit(uint stateBit)
        {
            uint newState = 0;
            lock (StateLock)
            {
                state |= stateBit;
                newState = state;
            }
            return newState;
        }
        public uint ClearStateBit(uint stateBit)
        {
            uint newState = 0;
            lock (StateLock)
            {
                state &= ~stateBit;
                newState = state;
            }
            return newState;
        }
        public bool IsStealthed()
        {
            bool isStealthed = false;
            lock (StateLock)
            {
                if ((state & 0x8) == 0x8)
                    isStealthed = true;
            }
            return isStealthed;
        }

        public InventoryItem GetInventoryItem(byte storageType, byte storageId, short storageSlot)
        {
            InventoryItem invItem = inventoryItems.Find(x => x.StorageType == storageType && x.StorageId == storageId && x.StorageSlot == storageSlot);
            return invItem;
        }

        public InventoryItem GetInventoryItem(ulong instanceId)
        {
            InventoryItem invItem = inventoryItems.Find(x => x.StorageItem.InstanceId == instanceId);
            return invItem;
        }

        public InventoryItem GetInventoryItem(Item item, byte canHold = 0) // If just need to get the item only pass item, if looking for a stack to add looted/traded items use canHold
        {
            InventoryItem invItem = inventoryItems.Find(x => (x.StorageItem == item) && (x.StorageCount + canHold <= 255));
            return invItem;
        }
        public void UpdateInventoryItem(InventoryItem invItem)
        {
            InventoryItem invItm = inventoryItems.Where(w => w.InstanceId == invItem.InstanceId).First();
            invItm.StorageId = invItem.StorageId;
            invItm.StorageItem = invItem.StorageItem;
            invItm.StorageSlot = invItem.StorageSlot;
            invItm.StorageType = invItem.StorageType;
        }
        public void RemoveInventoryItem(InventoryItem invItem)
        {
            InventoryItem invItm = inventoryItems.Where(w => w.InstanceId == invItem.InstanceId).First();
            inventoryItems.Remove(invItm);
        }
        public InventoryItem GetNextInventoryItem(NecServer server)
        {
            InventoryItem invItem = null;
            byte bagId = 0;
            short slotId = -1;
            foreach (Bag bag in inventoryBags)
            {
                bagId = bag.StorageId;
                List<InventoryItem> invItems = inventoryItems.FindAll(x => x.StorageId == bag.StorageId);
                if (invItems.Count == bag.NumSlots)
                    continue;
                else if (invItems.Count == 0)
                {
                    slotId = 0;
                    break;
                }
                bagId = bag.StorageId;
                short[] slots = invItems.Select(invItems => invItems.StorageSlot).OrderBy(StorageSlot => StorageSlot).ToArray();   // Get a sorted list of occupied slots
                short i = 0;
                foreach (short slot in slots)
                {
                    if (slot != i)
                    {
                        slotId = i;
                        break;
                    }
                    i++;
                }
                if (slotId == -1 && i < bag.NumSlots)
                {
                    slotId = i;
                }
            }
            if (slotId != -1)
            {
                invItem = server.Instances64.CreateInstance<InventoryItem>();
                invItem.StorageId = bagId;
                invItem.StorageSlot = slotId;
                inventoryItems.Add(invItem);
            }
            return invItem;
         }
    }
}
