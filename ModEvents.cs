using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters
{
    public class ModEvents
    {
        public static event EventHandler PlayerMessagesUpdated;
        public static event EventHandler PlayerCreated;

        internal static void RaisePlayerMessagesUpdatedEvent()
        {
            PlayerMessagesUpdated(null, null);
        }

        internal static void RaisePlayerCreatedEvent()
        {
            PlayerCreated(null, null);
        }
    }
}
