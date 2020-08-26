namespace Necromancy.Server.Model
{
    enum Status
    {

        Unidentified = 1 << 0, 

        Normal = 1 << 1, //does nothing use if you want a normal item no other flags

        Broken = 1 << 2,

        Cursed = 1 << 3,

        Blessed = 1 << 4

    }
}
