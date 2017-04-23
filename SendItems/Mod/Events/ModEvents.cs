using System;

namespace Denifia.Stardew.SendItems.Events
{
    public class SendItemsModEvents
    {
        public static event EventHandler<MailComposedEventArgs> MailComposed;
        public static event EventHandler PlayerCheckedLetterbox;
        public static event EventHandler OnMailDeliverySchedule;
        public static event EventHandler<MailReadEventArgs> MailRead;

        internal static void RaiseMailComposed(object sender, MailComposedEventArgs e)
        {
            MailComposed(sender, e);
        }

        internal static void RaisePlayerCheckedLetterbox(object sender, EventArgs e)
        {
            PlayerCheckedLetterbox(sender, e);
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
