using System;

namespace Necromancy.Server.Model.ItemModel
{
    public class Item
    {
        public Item()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType ItemType { get; set; }
        public EquipmentSlotType EquipmentSlotType { get; set; }
        public LoadEquipType LoadEquipType { get; set; }
        public int Physical { get; set; }
        public int Magical { get; set; }
        public int Durability { get; set; }


        public static EquipmentSlotType GetEquipmentSlotTypeBySlotNumber(int slotNumber)
        {
            EquipmentSlotType equipmentSlotType = EquipmentSlotType.NONE;
            if (slotNumber >= 0)
            {
                int value = 1 << slotNumber;
                if (Enum.IsDefined(typeof(EquipmentSlotType), value))
                {
                    equipmentSlotType = (EquipmentSlotType)value;
                }
            }

            return equipmentSlotType;
        }
    }
}
