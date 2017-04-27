using System;

namespace Denifia.Stardew.SendItems.Events
{
    public class ModEvents
    {
        public static event EventHandler<MailComposedEventArgs> MailComposed;
        public static event EventHandler PlayerUsingLetterbox;
        public static event EventHandler PlayerUsingPostbox;
        public static event EventHandler OnMailDeliverySchedule;
        public static event EventHandler MailDelivered;
        public static event EventHandler<MailReadEventArgs> MailRead;

        internal static void RaiseMailComposed(object sender, MailComposedEventArgs e)
        {
            MailComposed?.Invoke(sender, e);
        }

        internal static void RaisePlayerUsingLetterbox(object sender, EventArgs e)
        {
            PlayerUsingLetterbox?.Invoke(sender, e);
        }

        internal static void RaisePlayerUsingPostbox(object sender, EventArgs e)
        {
            PlayerUsingPostbox?.Invoke(sender, e);
        }

        internal static void RaiseOnMailDeliverySchedule(object sender, EventArgs e)
        {
            OnMailDeliverySchedule?.Invoke(sender, e);
        }

        internal static void RaiseMailDelivered(object sender, EventArgs e)
        {
            MailDelivered?.Invoke(sender, e);
        }

        internal static void RaiseMailRead(object sender, MailReadEventArgs e)
        {
            MailRead?.Invoke(sender, e);
        }
    }
}
