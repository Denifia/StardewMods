using StardewValley;
using System.Collections.Generic;
using Denifia.Stardew.SendItems.Domain;
using Denifia.Stardew.SendItems.Menus;
using Denifia.Stardew.SendItems.Events;
using System.Threading.Tasks;
using LiteDB;
using System;
using Denifia.Stardew.SendItems.Framework;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IPostboxService {

    }

    /// <summary>
    /// Handles what to do when a player uses the postbox and creates a letter
    /// </summary>
    public class PostboxService : IPostboxService
    {
        private const string _letterPostedNotification = "Letter Posted!";
        private const string _leaveSelectionKeyAndValue = "(Leave)";
        private const string _messageFormat = "Hey there!^^  I thought you might like this... Take care! ^    -{0} %item object {1} {2} %%";

        private readonly IFarmerService _farmerService;
        private readonly IConfigurationService _configService;

        public PostboxService(
            IConfigurationService configService,
            IFarmerService farmerService)
        {
            _configService = configService;
            _farmerService = farmerService;

            SendItemsModEvents.PlayerUsingPostbox += PlayerUsingPostbox;
            SendItemsModEvents.MailComposed += MailComposed;
        }

        private void PlayerUsingPostbox(object sender, EventArgs e)
        {
            Task.Run(DisplayFriendSelector);
        }

        private async Task DisplayFriendSelector()
        {
            if (Game1.activeClickableMenu != null) return;
            List<Response> responseList = new List<Response>();
            var farmers = await _farmerService.GetFarmersAsync();
            foreach (var friend in farmers)
            {
                responseList.Add(new Response(friend.Id, friend.DisplayText));
            }
            responseList.Add(new Response(_leaveSelectionKeyAndValue, _leaveSelectionKeyAndValue));
            Game1.currentLocation.createQuestionDialogue("Select Friend:", responseList.ToArray(), new GameLocation.afterQuestionBehavior(FriendSelectorAnswered), (NPC)null);
            Game1.player.Halt();
        }

        private void FriendSelectorAnswered(StardewValley.Farmer farmer, string answer)
        {
            if (answer.Equals(_leaveSelectionKeyAndValue)) return;

            var items = new List<Item> { null };
            Game1.activeClickableMenu = new ComposeLetter(answer, items, 1, 1, null, HighlightOnlyGiftableItems);
            // TODO: Should I use this instead?
            //Game1.activeClickableMenu = (IClickableMenu)new ComposeLetter(answer, items, 1, 1, new ComposeLetter.behaviorOnItemChange(onLetterChange)); 
        }

        private bool HighlightOnlyGiftableItems(Item i)
        {
            return i.canBeGivenAsGift();
        }

        private void MailComposed(object sender, MailComposedEventArgs e)
        {
            var toFarmerId = e.ToFarmerId;
            var fromFarmer = _farmerService.CurrentFarmer;
            var item = e.Item;

            if (item == null) return;

            var messageText = string.Format(_messageFormat, fromFarmer.Name, item.parentSheetIndex, item.getStack());

            // Consider: Moving this to own service
            Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    var mail = new Mail()
                    {
                        ToFarmerId = toFarmerId,
                        FromFarmerId = fromFarmer.Id,
                        Text = messageText,
                        CreatedDate = DateTime.Now,
                        Status = MailStatus.Composed
                    };

                    db.Insert(mail);
                    ModHelper.ShowInfoMessage(_letterPostedNotification);
                }
            }).Wait();
        }
    }
}
