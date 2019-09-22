using System;

namespace Necromancy.Server.Model
{
    public class Character
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SoulId { get; set; }
        public DateTime Created { get; set; }
  
        public string Name { get; set; }
        public uint Raceid { get; set; }
        public uint Sexid { get; set; }
        public byte HairId { get; set; }
        public byte HairColorId { get; set; }
        public byte FaceId { get; set; }
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

        public Character()
        {
            Id = -1;
            AccountId = -1;
            SoulId = -1;

            Level = 0;
            Name = null;
            Created = DateTime.Now;
        }

    }

}