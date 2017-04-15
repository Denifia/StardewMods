using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using denifia.stardew.sendletters.Domain;

namespace denifia.stardew.sendletters
{
    public class ModEvents
    {
        public delegate void MessageReadEventHandler(Message message);

        public static event EventHandler PlayerMessagesUpdated;
        public static event EventHandler PlayerCreated;
        public static event EventHandler MessageSent;
        public static event MessageReadEventHandler MessageRead;
        public static event EventHandler CheckMailbox;

        internal static void RaisePlayerMessagesUpdatedEvent()
        {
            PlayerMessagesUpdated(null, null);
        }

        internal static void RaisePlayerCreatedEvent()
        {
            PlayerCreated(null, null);
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
    }

    
}
