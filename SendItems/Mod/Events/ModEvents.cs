using System;

namespace Denifia.Stardew.SendItems.Events
{
    public class ModEvents
    {
        public static event EventHandler<MailComposedEventArgs> MailComposed;

        protected static void RaiseMailComposed(object sender, MailComposedEventArgs e)
        {
            MailComposed(sender, e);
        }
    }
}
