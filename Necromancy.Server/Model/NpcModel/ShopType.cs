namespace Necromancy.Server.Model
{
    enum ShopType
    {

        None = 1 << 0, 
        Curse = 1 << 0,
        Purchase = 1 << 1,
        Sell = 1 << 2,
        Identify = 1 << 3,
        Repair = 1 << 4,
        Special = 1 << 5,
        Meal = 1 << 6,
        Forge = 1 <<7,
        GemFusion = 0b01000000000,
        GemCombine = 0b10000000000,
        Donkey = Purchase + Sell + Identify,
        Blacksmith = Purchase + Identify + Repair + Forge,
        Gem = GemFusion + GemCombine,
        Bar = Meal

    }
}
