namespace Necromancy.Server.Model.ItemModel
{
    public enum ItemActionResultType
    {
        Ok = 0,
        ErrorGeneric = 1,
        ErrorBagLocation = -201, // Store location is incorrect
        ErrorAmount = -204, //Item amount is incorrect
        ErrorTarget = -205, //The target to use this item is incorrect
        ErrorDelay = -206, //Unable to use due to delay time
        ErrorSpace = -207, //No space available in inventory
        ErrorCursed = -208, //Unable to use this item since it is cursed
        ErrorBroken = -209, //Unable to use this item since it is broken
        ErrorRequirement = -210, //You do not meet the requirements to equip this item
        ErrorUse = -211, //Unable to use this item
        ErrorStatus = -212, //You are not in the right status to use this item
        ErrorCooldown = -230, //Unable to use since it is on cool down.
        ErrorAlreadyReceived = -2601, //You've already received this scrap
        ErrorOutsideTown = -2708, //Cannot be used outside of town
        ErrorShopOpen = -3001 //Unable to move items when you have a shop open
    }
}
