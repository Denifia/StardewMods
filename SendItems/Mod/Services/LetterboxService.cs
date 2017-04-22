using Denifia.Stardew.SendItems.Domain;
using Denifia.Stardew.SendItems.Events;
using LiteDB;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendItems.Services
{
    public interface ILetterboxService
    {

    }

    public class LetterboxService
    {
        private const string _playerMailKey = "playerMail";
        private const string _playerMailTitle = "Player Mail";

        private readonly IConfigurationService _configService;

        public LetterboxService(
            IConfigurationService configService)
        {
            _configService = configService;
            ModEvents.PlayerCheckedLetterbox += PlayerCheckedLetterbox;
        }

        private void PlayerCheckedLetterbox(object sender, EventArgs e) // TODO: Change event args to pass in current farmer id? or just use the farmer service?
        {
            Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    var mail = db.Query<Mail>().Where(x => x.Status == MailStatus.Posted).ToList();
                }

                var message = _messageService.GetFirstMessage(_playerService.CurrentPlayer.Id);
                if (message != null && !(Game1.mailbox == null || !Game1.mailbox.Any()))

                {
                    ShowLetter(message);
                }
                else
                {
                    if (!Game1.mailbox.Any())
                    {
                        ShowFriendSelecter();
                    }
                }
            }).Wait();
        }

        private void ShowLetter(Mail mail)
        {
            if (Game1.mailbox == null || !Game1.mailbox.Any()) return;
            if (Game1.mailbox.Peek() == _playerMailKey)
            {
                Game1.activeClickableMenu = new LetterViewerMenu(mail.Text, _playerMailTitle);
                if (Game1.mailbox.Any())
                {
                    Game1.mailbox.Dequeue();
                }
                ModEvents.RaiseMessageReadEvent(mail);
            }
        }
    }
}
