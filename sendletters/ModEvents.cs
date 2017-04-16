using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using denifia.stardew.sendletters.Domain;
using StardewValley;
using denifia.stardew.common.Domain;

namespace denifia.stardew.sendletters
{
    public class ModEvents
    {
        public delegate void MessageReadEventHandler(Message message);
        public delegate void PlayerCreatedHandler(Player player);
        public delegate void MessageCraftedHandler(string toPlayerId, Item item);

        public static event EventHandler PlayerMessagesUpdated;
        public static event PlayerCreatedHandler PlayerCreated;
        public static event EventHandler MessageSent;
        public static event MessageReadEventHandler MessageRead;
        public static event EventHandler CheckMailbox;
        public static event MessageCraftedHandler MessageCrafted;

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
        
        internal static void RaiseMessageCraftedEvent(string toPlayerId, Item item)
        {
            MessageCrafted(toPlayerId, item);
        }

    }

    
}
