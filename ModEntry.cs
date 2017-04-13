using RestSharp;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace denifia.stardew.sendletters
{
    public class ModEntry : Mod
    {
        private Player Player { get; set; }
        private RestClient RestClient { get; set; }
        private int LastMessageIndex = -1;

        public override void Entry(IModHelper helper)
        {
            Player = new Player
            {
                Id = "123",
                Name = "rat"
            };

            RestClient = new RestClient("http://localhost:53983/api/");

            SaveEvents.AfterLoad += SaveEvents_AfterLoad;
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;
            SaveEvents.AfterLoad -= SaveEvents_AfterLoad;
        }

        private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            CheckForMessages();
        }

        public void ShowLetter(Message message)
        {
            Game1.activeClickableMenu = (IClickableMenu)new LetterViewerMenu(message.Text, "Player Mail");
        }

        private void CreatePlayer()
        {
            var newPlayer = new CreaterPlayerModel
            {
                Name = Player.Name
            };
            
            var request = new RestRequest("players/{id}", Method.PUT);
            request.AddHeader("Content-type", "application/json; charset=utf-8");
            request.AddUrlSegment("id", Player.Id);
            request.AddJsonBody(newPlayer);

            RestClient.ExecuteAsync(request, response => {
                var x = response.Content;
            });
        }

        private void CheckForMessages()
        {
            var request = new RestRequest("messages/{id}", Method.GET);
            request.AddUrlSegment("id", Player.Id);

            RestClient.ExecuteAsync<List<Message>>(request, response => {
                Player.Messages = response.Data;
                if (Player.Messages.Any())
                {
                    var currentIndex = Player.Messages.Count - 1;

                    if (currentIndex > LastMessageIndex)
                    {
                        ShowLetter(Player.Messages[currentIndex]);
                        LastMessageIndex = currentIndex;
                    }
                }
            });
        }
    }
}
