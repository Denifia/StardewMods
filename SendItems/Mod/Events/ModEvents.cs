using System;

namespace Denifia.Stardew.SendItems.Events
{
    public class ModEvents
    {
        public static event EventHandler<MailComposedEventArgs> MailComposed;
        public static event EventHandler PlayerCheckedLetterbox;
        public static event EventHandler OnMailDeliverySchedule;

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
    }
}
