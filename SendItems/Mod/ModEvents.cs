using System;
using StardewValley;
using Denifia.Stardew.SendItems.Domain;

namespace Denifia.Stardew.SendItems
{
    public class ModEvents
    {
        public delegate void AfterMailComposedHandler(string toFarmerId, Item item);

        public static event AfterMailComposedHandler AfterMailComposed;
        
        internal static void InvokeAfterMailComposed(string toFarmerId, Item item)
        {
            AfterMailComposed(toFarmerId, item);
        }

    }

    
}
