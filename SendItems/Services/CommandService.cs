using Denifia.Stardew.SendItems.Domain;
using LiteDB;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendItems.Services
{
    public interface ICommandService
    {
        void RegisterCommands();
    }

    public class CommandService : ICommandService
    {
        private bool SavedGameLoaded = false;

        private readonly IMod _mod;
        private readonly IConfigurationService _configService;
        private readonly IFarmerService _farmerService;

        public CommandService(
            IMod mod, 
            IConfigurationService configService,
            IFarmerService farmerService)
        {
            _mod = mod;
            _configService = configService;
            _farmerService = farmerService;

            SaveEvents.AfterLoad += AfterSavedGameLoad;
        }        

        public void RegisterCommands()
        {
            _mod.Helper.ConsoleCommands
                .Add("temp", "", HandleCommand)
                .Add("SendItems_Me", "Shows you the command that your friends need to type to add the current farmer as a friend. \n\nUsage: SendItems_Me", HandleCommand)
                .Add("SendItems_ListLocalFarmers", "Lists all the local farmers (saved games). \n\nUsage: SendItems_ListLocalFarmers", HandleCommand)
                .Add("SendItems_AddAllLocalFarmersAsFriends", "Adds all the local farmers (saved games) as friends to the current farmer. \n\nUsage: SendItems_AddAllLocalFarmersAsFriends", HandleCommand)
                .Add("SendItems_AddFriend", "Adds a friend to the current farmer. \n\nUsage: SendItems_AddFriend <id> <name> <farmName> \n- id: the id of your friend.\n- name: the name of your friend.\n- farmName: the name of your friends farm.", HandleCommand)
                .Add("SendItems_RemoveFriend", "Removes a friend of the current farmer. \n\nUsage: SendItems_RemoveFriend <id> \n- id: id of your friend.", HandleCommand)
                .Add("SendItems_RemoveAllFriend", "Removes all the friends of the current farmer. \n\nUsage: SendItems_RemoveAllFriend", HandleCommand)
                .Add("SendItems_ListMyFriends", "Lists all the friends of the current farmer. \n\nUsage: SendItems_ListLocalFarmers", HandleCommand);
        }

        private void HandleCommand(string command, string[] args)
        {
            if (!SavedGameLoaded)
            {
                _mod.Monitor.Log("Please load up a saved game first, then try again.", LogLevel.Warn);
                return;
            }

            // TODO: Split these into methods
            switch (command)
            {
                case "temp":

                    using (var db = new LiteRepository(_configService.ConnectionString))
                    {
                        db.Insert(new Mail()
                        {
                            ToFarmerId = "150965384",
                            FromFarmerId = "150965384",
                            Text = "Hi2",
                            Status = MailStatus.Composed,
                            CreatedDate = DateTime.Now
                        });
                    }

                    break;
                case "sendletters_me":
                    _mod.Monitor.Log("Command for others to add the currently loaded farmer as a friend is...", LogLevel.Info);
                    _mod.Monitor.Log($"sendletters_addfriend -Name {_farmerService.CurrentFarmer.Name} -FarmName {_farmerService.CurrentFarmer.FarmName} -Id {_farmerService.CurrentFarmer.Id}", LogLevel.Info);
                    _mod.Monitor.Log("Feel free to change your <Name> if you want but the <Id> needs to stay as it is.", LogLevel.Info);
                    break;
                case "sendletters_friends":
                    var friends = _farmerService.CurrentFarmer.Friends;
                    if (friends.Any())
                    {
                        _mod.Monitor.Log("Your friends for the currently loaded farmer are...", LogLevel.Info);
                        foreach (var friend in friends)
                        {
                            _mod.Monitor.Log($"{friend.Name} ({friend.FarmName} Farm) [ID: {friend.Id}]", LogLevel.Info);
                        }
                    }
                    else
                    {
                        _mod.Monitor.Log("The currently loaded farmer has no friends. How sad.", LogLevel.Info);
                        _mod.Monitor.Log("You can add friends with the sendletters_addfriend command.", LogLevel.Info);
                    }
                    break;
                case "sendletters_addfriend":
                    if (args.Length == 6 && args[0].ToLower() == "-name" && args[2].ToLower() == "-farmname" && args[4].ToLower() == "-id")
                    {
                        var name = args[1];
                        var farmName = args[3];
                        var id = args[5];
                        //_farmerService.AddFriendToCurrentPlayer(name, farmName, id); // TODO: Replace
                        _mod.Monitor.Log($"{name} ({farmName} Farm) was added!", LogLevel.Info);
                    }
                    else
                    {
                        LogArgumentsInvalid(command);
                    }
                    break;
                case "sendletters_removefriend":
                    if (args.Length == 2 && args[0].ToLower() == "-id")
                    {
                        var id = args[1];
                        var friend = _farmerService.CurrentFarmer.Friends.FirstOrDefault(x => x.Id == id);
                        if (friend != null)
                        {
                            //_farmerService.RemoveFriendFromCurrentPlayer(id); // TODO: replace
                            _mod.Monitor.Log($"{friend.Name} ({friend.FarmName} Farm) was removed!", LogLevel.Info);
                        }
                        else
                        {
                            _mod.Monitor.Log($"Couldn'd find a friend with that id.", LogLevel.Info);
                        }
                    }
                    else
                    {
                        LogArgumentsInvalid(command);
                    }
                    break;
                default:
                    throw new NotImplementedException($"SendLetters received unknown command '{command}'.");
            }
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
            SavedGameLoaded = true;
        }

        /****
        ** Logging
        ****/
        private void LogUsageError(string error, string command)
        {
            _mod.Monitor.Log($"{error} Type 'help {command}' for usage.", LogLevel.Error);
        }

        private void LogArgumentsInvalid(string command)
        {
            LogUsageError("The arguments are invalid.", command);
        }
    }
}
