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
        public int Level { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte viewOffset { get; set; }
        public int battlePose { get; set; }
        public int charaPose { get; set; }

        public Character()
        {
            Id = -1;
            AccountId = -1;
            SoulId = -1;
        }
    }
}