using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class NpcSpawn : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int NpcId { get; set; }
        public string Name { get; set; }

        public int StatusEffectId { get; set; }
        public float StatusEffectX { get; set; }
        public float StatusEffectY { get; set; }
        public float StatusEffectZ { get; set; }
        public string ToDo { get; set; }
        public int SpecialAttribute { get; set; }
        public int EventFirstEncounter { get; set; }
        public int EventAlways { get; set; }
        public int PlayCutsceneFirstEncounter { get; set; }
        public int PlayCutsceneAlways { get; set; }
        public byte Level { get; set; }
        public string Title { get; set; }
        public int DragonStatueType { get; set; }
        public int IconType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int MapId { get; set; }
        public int DisplayConditionFlag { get; set; }
        public int SplitMapNumber { get; set; }
        public int SettingTypeFlag { get; set; }
        public int ModelId { get; set; }
        public short Radius { get; set; }
        public byte Height { get; set; }
        public byte CrouchHeight { get; set; }
        public byte NamePlate { get; set; }
        public int HeightModelAttribute { get; set; }
        public int ZOffset { get; set; }
        public int EffectScaling { get; set; }
        public byte Heading { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }


        public NpcSpawn()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }
    }
}
