using System;

namespace Necromancy.Server.Model
{
    public class Character
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SoulId { get; set; }
        public DateTime Created { get; set; }

        public byte Characterslotid { get; set; }
        public string Name { get; set; }

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
        public byte Level { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte viewOffset { get; set; }
        public int battlePose { get; set; }
        public int charaPose { get; set; }

        public byte movementAnim { get; set; }
        public byte animJumpFall { get; set; }
        public byte xAnim { get; set; }
        public bool weaponEquipped { get; set; }
        public byte wepEastWestAnim { get; set; }
        public byte wepNorthSouthAnim { get; set; }
        public byte logoutCanceled { get; set; }
        public int MapId { get; set; }
        public bool NewCharaProtocol { get; set; }
        public int WeaponType { get; set; }
        public int AdventureBagGold { get; set; }
        public byte soulFormState { get; set; }
        public int[] EquipId { get; set; }
        public int selectExecCode { get; set; }



        public Character()
        {
            Id = -1;
            AccountId = -1;
            SoulId = -1;
            Characterslotid = 0;
            Level = 0;
            Name = null;
            Created = DateTime.Now;
            logoutCanceled = 0;
            NewCharaProtocol = false;
            WeaponType = 8;
            AdventureBagGold = 80706050;
            selectExecCode = -1;
        }
    }

}
