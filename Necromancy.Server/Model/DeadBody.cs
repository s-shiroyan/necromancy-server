using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class DeadBody : IInstance
    {
        public uint InstanceId { get; set; }
        public int CharacterInstanceId { get; set; }
        public int Id { get; set; }
        public string CharaName { get; set; }
        public string SoulName { get; set; }
        public string Title { get; set; }
        public int MapId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public uint RaceId { get; set; }
        public uint SexId { get; set; }
        public byte HairStyle { get; set; }
        public byte HairColor { get; set; }
        public byte FaceId { get; set; }
        public int ConnectionState { get; set; }
        public int ModelType { get; set; }
        public byte CriminalStatus { get; set; }
        public byte BeginnerProtection { get; set; }
        public int deathPose { get; set; }


        public DeadBody()
        {
            /*InstanceId = CharacterId.InstanceId;
            CharaName = CharacterId.Name;
            MapId = CharacterId.MapId;
            X = CharacterId.X;
            Y = CharacterId.Y;
            Z = CharacterId.Z;
            Heading = CharacterId.Heading;
            RaceId = CharacterId.Raceid;
            SexId = CharacterId.Sexid;
            HairStyle = CharacterId.HairId;
            HairColor = CharacterId.HairColorId;
            FaceId = CharacterId.FaceId;
            */
            ConnectionState = 1;//0 if disconnected, 1 if dead.
            ModelType = 1; //4 if they are an ash pile
            CriminalStatus = 0; //We need a criminal status value from original character
            BeginnerProtection = 1; // We need a beginner protection value from original character
            deathPose = 1; // We need to send whatever value our character dies with here, 1 = head popped off, 4 = chopped in half (this should come from recv_battle_report_noact_notify_dead)*/
        }
    }
}
