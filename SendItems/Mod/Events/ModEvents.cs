using System;

namespace Denifia.Stardew.SendItems.Events
{
    public class SendItemsModEvents
    {
        public static event EventHandler<MailComposedEventArgs> MailComposed;
        public static event EventHandler PlayerUsingLetterbox;
        public static event EventHandler PlayerUsingPostbox;
        public static event EventHandler OnMailDeliverySchedule;
        public static event EventHandler<MailReadEventArgs> MailRead;

        internal static void RaiseMailComposed(object sender, MailComposedEventArgs e)
        {
            MailComposed(sender, e);
        }

        internal static void RaisePlayerUsingLetterbox(object sender, EventArgs e)
        {
            PlayerUsingLetterbox(sender, e);
        }

        internal static void RaisePlayerUsingPostbox(object sender, EventArgs e)
        {
            PlayerUsingPostbox(sender, e);
        }

        internal static void RaiseOnMailDeliverySchedule(object sender, EventArgs e)
        {
            OnMailDeliverySchedule(sender, e);
        }

        internal static void RaiseMailRead(object sender, MailReadEventArgs e)
        {
            MailRead(sender, e);
        }
    }
}
