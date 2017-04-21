using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Denifia.Stardew.SendLetters.Domain;
using StardewValley;
using Denifia.Stardew.SendLetters.Common.Domain;

namespace Denifia.Stardew.SendLetters
{
    public class ModEvents
    {
        public delegate void MessageReadEventHandler(Message message);
        public delegate void PlayerCreatedHandler(Player player);
        public delegate void AfterMailComposedHandler(string toFarmerId, Item item);

        public static event EventHandler PlayerMessagesUpdated;
        public static event PlayerCreatedHandler PlayerCreated;
        public static event EventHandler MessageSent;
        public static event MessageReadEventHandler MessageRead;
        public static event EventHandler CheckMailbox;
        public static event AfterMailComposedHandler AfterMailComposed;

        internal static void RaisePlayerMessagesUpdatedEvent()
        {
            PlayerMessagesUpdated(null, null);
        }

        internal static void RaisePlayerCreatedEvent(Player player)
        {
            PlayerCreated(player);
        }

        internal static void RaiseMessageSentEvent()
        {
            MessageSent(null, null);
        }

        internal static void RaiseMessageReadEvent(Message message)
        {
            MessageRead(message);
        }

        internal static void RaiseCheckMailboxEvent()
        {
            CheckMailbox(null, null);
        }
        
        internal static void InvokeAfterMailComposed(string toFarmerId, Item item)
        {
            AfterMailComposed(toFarmerId, item);
        }

    }

    
}
