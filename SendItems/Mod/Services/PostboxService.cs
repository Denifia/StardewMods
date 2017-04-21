using Denifia.Stardew.SendLetters;
using Denifia.Stardew.SendLetters.Domain;
using Denifia.Stardew.SendLetters.Menus;
using StardewValley;
using System.Collections.Generic;

namespace Denifia.Stardew.SendItems.Services
{
    public class PostboxService
    {
        private const string _leaveSelectionKeyAndValue = "(Leave)";
        private const string _messageFormat = "Hey there!^^  I thought you might like this... Take care! ^    -{0} %item object {1} {2} %%";

        public PostboxService()
        {
            ModEvents.AfterMailComposed += AfterMailComposed;
        }

        public void ShowComposeMailUI()
        {
            DisplayFriendSelector();
        }

        private void DisplayFriendSelector()
        {
            if (Game1.activeClickableMenu != null) return;
            List<Response> responseList = new List<Response>();
            foreach (var friend in new List<Friend>()) // TODO: Replace with real list
            {
                responseList.Add(new Response(friend.Id, friend.DisplayText));
            }
            responseList.Add(new Response(_leaveSelectionKeyAndValue, _leaveSelectionKeyAndValue));
            Game1.currentLocation.createQuestionDialogue("Select Friend:", responseList.ToArray(), new GameLocation.afterQuestionBehavior(FriendSelectorAnswered), (NPC)null);
            Game1.player.Halt();
        }

        private void FriendSelectorAnswered(Farmer who, string answer)
        {
            if (!answer.Equals(_leaveSelectionKeyAndValue))
            {
                var items = new List<Item>
                {
                    null
                };
                Game1.activeClickableMenu = new ComposeLetter(answer, items, 1, 1, null, HighlightOnlyGiftableItems);
                //Game1.activeClickableMenu = (IClickableMenu)new ComposeLetter(answer, items, 1, 1, new ComposeLetter.behaviorOnItemChange(onLetterChange)); // TODO: Should I use this instead?
            }
        }

        private bool HighlightOnlyGiftableItems(Item i)
        {
            return i.canBeGivenAsGift();
        }

        private void AfterMailComposed(string toFarmerId, Item item)
        {
            if (item == null) return;

            var messageText = string.Format(_messageFormat, "farmerName", item.parentSheetIndex, item.getStack());
            
            // TODO: Create mail in local DB and set it to Posted
        }
    }
}
