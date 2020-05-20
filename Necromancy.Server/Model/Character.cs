using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model.Stats;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Model
{
    public class Character : IInstance
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(Character));

        private readonly object StateLock = new object();
        private readonly object HPLock = new object();
        private readonly object DamageLock = new object();
        private readonly object MPLock = new object();
        private readonly object ODLock = new object();
        public uint InstanceId { get; set; }

        //core attributes
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SoulId { get; set; }
        public DateTime Created { get; set; }
        public byte Slot { get; set; }
        public string Name { get; set; }
        public string SoulName { get; set; }
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

        //public int maxHp { get; set; }
        //public int maxMp { get; set; }
        //public int maxOd { get; set; }
        public bool hadDied { get; set; }
        public uint DeadBodyInstanceId { get; set; }
        public int Channel { get; set; }
        public int beginnerProtection { get; set; }
        public uint _state { get; set; }

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
        public bool mapChange { get; set; }

        //Normal Attack
        public int[] AttackIds { get; set; }
        //Logout Cancel Detection
        //public byte logoutCanceled { get; set; }

        //Map Related
        public int MapId { get; set; }

        //Temporary Value Holders
        public int WeaponType { get; set; }
        public int AdventureBagGold { get; set; }
        public byte soulFormState { get; set; }
        public int[] EquipId { get; set; }
        public uint activeSkillInstance { get; set; }
        public bool castingSkill { get; set; }
        public uint eventSelectReadyCode { get; set; }
        public int eventSelectExecCode { get; set; }

        public int eventSelectExtraSelectionCode { get; set; }

        //private int _currentHp { get; set; }
        public BaseStat Hp;
        public BaseStat Mp;

        public BaseStat Od;

        //private int _currentMp { get; set; }
        //private int _currentOd { get; set; }
        public int shortcutBar0Id { get; set; }
        public int shortcutBar1Id { get; set; }
        public int shortcutBar2Id { get; set; }
        public int shortcutBar3Id { get; set; }
        public int shortcutBar4Id { get; set; }
        public List<InventoryItem> inventoryItems { get; set; }
        public List<Bag> inventoryBags { get; set; }
        public InventoryItem[] equipSlots { get; set; }
        public bool takeover { get; set; }
        public int skillStartCast { get; set; }
        public bool helperText { get; set; }
        public bool helperTextBlacksmith { get; set; }
        public bool helperTextDonkey { get; set; }
        public bool helperTextCloakRoom { get; set; }
        public bool helperTextAbdul { get; set; }
        public Event currentEvent { get; set; }
        public bool secondInnAccess { get; set; }

        public uint killerInstanceId { get; private set; }

        //public bool playerDead { get; set; }
        public uint partyId { get; set; }
        public int unionId { get; set; }
        public byte criminalState { get; set; }

        //Msg Value Holders
        public uint friendRequest { get; set; }
        public uint partyRequest { get; set; }

        //Task
        public CharacterTask characterTask;
        public bool _characterActive { get; private set; }

        //Flags  
        [Flags]
        public enum CharacterState
        {
            //state            //bitShift           // Binary                           Dec
            SoulForm = 0, // 0000 0000 0000 0000 0000 0000    0
            BattlePose = 1 << 0, // 0000 0000 0000 0000 0000 0001    1
            BlockPose = 1 << 1, // 0000 0000 0000 0000 0000 0010    2
            StealthForm = 1 << 2, // 0000 0000 0000 0000 0000 0100    4
            NothingForm = 1 << 3, // 0000 0000 0000 0000 0000 1000    8
            NormalForm = 1 << 4, // 0000 0000 0000 0000 0001 0000    16
            InvisibleForm = 1 << 5, // 0000 0000 0000 0000 0010 0000    32 
            InvulnerableForm = 1 << 6, // 0000 0000 0000 0000 0100 0000    64 
            GameMaster = 1 << 12, // 0000 0000 0001 0000 0000 0000    4096 
            RequestPartyJoin = 1 << 13, // 0000 0000 0010 0000 0000 0000    8192 
            RecruitPartyMember = 1 << 14, // 0000 0000 0100 0000 0000 0000    16384 
            LostState = 1 << 15, // 0000 0000 1000 0000 0000 0000    32768
            HeadState = 1 << 16, // 0000 0001 0000 0000 0000 0000    65536
            MemberBonus = 1 << 20, // 0001 0000 0000 0000 0000 0000‬    1048576
        }

        [Flags]
        public enum BodyState
        {
            //state            //bitShift            // Binary                           Dec
            SoulForm = 0, // 0000 0000 0000 0000 0000 0000    0
            NormalDeadBody = 1 << 0, // 0000 0000 0000 0000 0000 0001    1
            RuckSack = 1 << 1, // 0000 0000 0000 0000 0000 0010    2
            CollectedBody = 1 << 2, // 0000 0000 0000 0000 0000 0100    4
            RuckSackAlso = 1 << 3, // 0000 0000 0000 0000 0000 1000    8
        }


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
            WeaponType = 8;
            AdventureBagGold = 80706050;
            eventSelectExecCode = -1;
            Hp = new BaseStat(1000, 1000);
            Mp = new BaseStat(450, 500);
            Od = new BaseStat(150, 200);
            shortcutBar0Id = -1;
            shortcutBar1Id = -1;
            shortcutBar2Id = -1;
            shortcutBar3Id = -1;
            shortcutBar4Id = -1;
            takeover = false;
            skillStartCast = 0;
            battleAnim = 0;
            hadDied = false;
            _state = (int) CharacterState.NormalForm;
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
            _characterActive = true;
            killerInstanceId = 0;
            secondInnAccess = false;
            partyId = 0;
            InstanceId = 0;
            Name = "";
            ClassId = 0;
            unionId = 0;
            criminalState = 0;
            helperTextAbdul = true;
            mapChange = false;
            equipSlots = new InventoryItem[19];
        }

        public bool characterActive
        {
            get => _characterActive;
            set { _characterActive = value; }
        }

        public void CreateTask(NecServer server, NecClient client)
        {
            characterTask = new CharacterTask(server, client);
            characterTask.Start();
        }

        public uint state
        {
            get => _state;
            set
            {
                lock (StateLock)
                {
                    _state = value;
                }
            }
        }

        public uint AddStateBit(uint stateBit)
        {
            uint newState = 0;
            lock (StateLock)
            {
                _state |= stateBit;
                newState = _state;
            }

            return newState;
        }

        public uint ClearStateBit(uint stateBit)
        {
            uint newState = 0;
            lock (StateLock)
            {
                _state &= ~stateBit;
                newState = _state;
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
            InventoryItem invItem = inventoryItems.Find(x =>
                x.StorageType == storageType && x.StorageId == storageId && x.StorageSlot == storageSlot);
            return invItem;
        }

        public InventoryItem GetInventoryItem(ulong instanceId)
        {
            InventoryItem invItem = inventoryItems.Find(x => x.StorageItem.InstanceId == instanceId);
            return invItem;
        }

        public InventoryItem
            GetInventoryItem(Item item,
                byte canHold =
                    0) // If just need to get the item only pass item, if looking for a stack to add looted/traded items use canHold
        {
            InventoryItem invItem =
                inventoryItems.Find(x => (x.StorageItem == item) && (x.StorageCount + canHold <= 255));
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

        public InventoryItem GetNextInventoryItem(NecServer server, byte desiredCount = 1, Item item = null)
        {
            InventoryItem invItem = null;
            if (item != null)
            {
                invItem = GetInventoryItem(item, desiredCount);
                if (invItem != null)
                    return invItem;
            }

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
                short[] slots = invItems.Select(invItems => invItems.StorageSlot).OrderBy(StorageSlot => StorageSlot)
                    .ToArray(); // Get a sorted list of occupied slots
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
