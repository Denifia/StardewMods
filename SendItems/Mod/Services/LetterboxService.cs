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

    /// <summary>
    /// Handles what to do when a player uses the letter box and reads letters
    /// </summary>
    public class LetterboxService : ILetterboxService
    {
        private const string _playerMailKey = "playerMail";
        private const string _playerMailTitle = "Player Mail";

        private readonly IConfigurationService _configService;
        private readonly IFarmerService _farmerService;

        public LetterboxService(
            IConfigurationService configService,
            IFarmerService farmerService)
        {
            _configService = configService;
            _farmerService = farmerService;

            SendItemsModEvents.PlayerUsingLetterbox += PlayerUsingLetterbox;
            SendItemsModEvents.MailRead += MailRead;
        }

        private void PlayerUsingLetterbox(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    var currentFarmerId = _farmerService.CurrentFarmer.Id;
                    var mail = db.Query<Mail>().Where(x => x.Status == MailStatus.Delivered && x.ToFarmerId == currentFarmerId).FirstOrDefault();
                    if (mail != null && !(Game1.mailbox == null || !Game1.mailbox.Any()))
                    {
                        ShowLetter(mail);
                    }
                }
            });
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
                SendItemsModEvents.RaiseMailRead(this, new MailReadEventArgs { Id = mail.Id });
            }
        }

        private void MailRead(object sender, MailReadEventArgs e)
        {
            Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    var currentFarmerId = _farmerService.CurrentFarmer.Id;
                    db.Delete<Mail>(x => x.Id == e.Id);
                }
            });
        }
    }
}
