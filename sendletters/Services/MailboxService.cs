using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.Menus;
using denifia.stardew.sendletters.Models;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public class MailboxService : IMailboxService
    {
        private readonly IPlayerService _playerService;
        private readonly IMessageService _messageService;
        private const string _playerMailKey = "playerMail";
        private const string _playerMailTitle = "Player Mail";
        private const string _leaveSelectionKeyAndValue = "(Leave)";
        private const string _messageFormat = "Hey there!^^  I thought you might like this... Take care! ^    -{0} %item object {1} {2} %%";

        public MailboxService(IPlayerService playerService, IMessageService messageService)
        {
            _playerService = playerService;
            _messageService = messageService;

            ModEvents.MessageCrafted += AfterMessageCrafted;
            ModEvents.CheckMailbox += OnCheckMailbox;
        }

        public void ShowLetter(Message message)
        {
            if (Game1.mailbox == null || !Game1.mailbox.Any()) return;

            if (Game1.mailbox.Peek() == _playerMailKey)
            {
                Game1.activeClickableMenu = (IClickableMenu)new LetterViewerMenu(message.Text, _playerMailTitle);
                if (Game1.mailbox.Any())
                {
                    Game1.mailbox.Dequeue();
                }
                ModEvents.RaiseMessageReadEvent(message);
            }
        }

        public void PostLetters(int count)
        {
            while (Game1.mailbox.Any() && Game1.mailbox.Peek() == _playerMailKey)
            {
                Game1.mailbox.Dequeue();
            }

            for (int i = 0; i < count; i++)
            {
                Game1.mailbox.Enqueue(_playerMailKey);
            }
        }

        public void ShowFriendSelecter()
        {
            List<Response> responseList = new List<Response>();
            foreach (var friend in _playerService.GetCurrentPlayer().Friends)
            {
                responseList.Add(new Response(friend.Id, string.Format("{0} ({1})", friend.Name, friend.FarmName)));
            }
            responseList.Add(new Response(_leaveSelectionKeyAndValue, _leaveSelectionKeyAndValue));
            Game1.currentLocation.createQuestionDialogue("Select Friend:", responseList.ToArray(), new GameLocation.afterQuestionBehavior(this.SelectItem), (NPC)null);
            Game1.player.Halt();
        }

        private void SelectItem(Farmer who, string answer)
        {
            if (!answer.Equals(_leaveSelectionKeyAndValue))
            {
                Game1.activeClickableMenu = (IClickableMenu)new ComposeLetter(answer, new List<Item>(), 1, 1);
            }
        }

        private void OnCheckMailbox(object sender, EventArgs e)
        {
            var message = _messageService.GetFirstMessage(_playerService.GetCurrentPlayer().Id);
            if (message != null)
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
        }

        private void AfterMessageCrafted(string toPlayerId, Item item)
        {
            var currentPlayer = _playerService.GetCurrentPlayer();
            var messageText = string.Format(_messageFormat, currentPlayer.Name, item.parentSheetIndex, item.getStack());
            var newMessage = new MessageCreateMessage
            {
                FromPlayerId = currentPlayer.Id,
                ToPlayerId = toPlayerId,
                Text = messageText
            };

            _messageService.CreateMessage(newMessage);
            //Game1.player.removeItemsFromInventory(item.parentSheetIndex, item.getStack());
        }
    }
}
