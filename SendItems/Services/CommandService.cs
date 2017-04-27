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
    }

    public class CommandService : ICommandService
    {
        private bool SavedGameLoaded = false;

        private readonly IMod _mod;
        private readonly IConfigurationService _configService;
        private readonly IFarmerService _farmerService;

        private const string _meCommand = "SendItems_Me";
        private const string _listLocalFarmersCommand = "SendItems_ListLocalFarmers";
        private const string _addAllLocalFarmersAsFriendsCommand = "SendItems_AddAllLocalFarmersAsFriends";
        private const string _addFriendCommand = "SendItems_AddFriend";
        private const string _removeFriendCommand = "SendItems_RemoveFriend";
        private const string _removeAllFriendCommand = "SendItems_RemoveAllFriend";
        private const string _listMyFriendsCommand = "SendItems_ListMyFriends";

        public CommandService(
            IMod mod, 
            IConfigurationService configService,
            IFarmerService farmerService)
        {
            _mod = mod;
            _configService = configService;
            _farmerService = farmerService;

            RegisterCommands();

            SaveEvents.AfterLoad += AfterSavedGameLoad;
        }        

        private void RegisterCommands()
        {
            _mod.Helper.ConsoleCommands
                // TODO: Remove before go live
                .Add("temp", "", HandleCommand)
                .Add(_meCommand, "Shows you the command that your friends need to type to add the current farmer as a friend. \n\nUsage: SendItems_Me", HandleCommand)
                .Add(_listLocalFarmersCommand, "Lists all the local farmers (saved games). \n\nUsage: SendItems_ListLocalFarmers", HandleCommand)
                .Add(_addAllLocalFarmersAsFriendsCommand, "Adds all the local farmers (saved games) as friends to the current farmer. \n\nUsage: SendItems_AddAllLocalFarmersAsFriends", HandleCommand)
                .Add(_addFriendCommand, "Adds a friend to the current farmer. \n\nUsage: SendItems_AddFriend <id> <name> <farmName> \n- id: the id of your friend.\n- name: the name of your friend.\n- farmName: the name of your friends farm.", HandleCommand)
                .Add(_removeFriendCommand, "Removes a friend of the current farmer. \n\nUsage: SendItems_RemoveFriend <id> \n- id: id of your friend.", HandleCommand)
                .Add(_removeAllFriendCommand, "Removes all the friends of the current farmer. \n\nUsage: SendItems_RemoveAllFriend", HandleCommand)
                .Add(_listMyFriendsCommand, "Lists all the friends of the current farmer. \n\nUsage: SendItems_ListLocalFarmers", HandleCommand);
        }

        private void HandleCommand(string command, string[] args)
        {
            if (!SavedGameLoaded)
            {
                _mod.Monitor.Log("Please load up a saved game first, then try again.", LogLevel.Warn);
                return;
            }

            switch (command)
            {
                case "temp":
                    Temp(args);
                    break;
                case _meCommand:
                    Me(args);
                    break;
                case _listLocalFarmersCommand:
                    ListLocalFarmers(args);
                    break;
                case _addAllLocalFarmersAsFriendsCommand:
                    AddAllLocalFarmersAsFriends(args);
                    break;
                case _addFriendCommand:
                    AddFriend(args);
                    break;
                case _removeFriendCommand:
                    RemoveFriend(args);
                    break;
                case _removeAllFriendCommand:
                    RemoveAllFriend(args);
                    break;
                case _listMyFriendsCommand:
                    ListMyFriends(args);
                    break;
                default:
                    throw new NotImplementedException($"Send Items received unknown command '{command}'.");
            }
        }

        private void Temp(string[] args)
        {
            var mail = new Mail()
            {
                ToFarmerId = "150965384",
                FromFarmerId = "150965384",
                Text = "Hi2",
                Status = MailStatus.Composed,
                CreatedDate = DateTime.Now
            };
            Repository.Instance.Insert(mail);
        }

        private void Me(string[] args)
        {
            _mod.Monitor.Log("Get your friends to paste this into the SMAPI console to add you as a friend. Each farmer (saved game) has it's own list of friends.", LogLevel.Info);
            _mod.Monitor.Log($"{_addFriendCommand} {_farmerService.CurrentFarmer.Id} {_farmerService.CurrentFarmer.Name} {_farmerService.CurrentFarmer.FarmName}", LogLevel.Info);
        }

        private void ListLocalFarmers(string[] args)
        {

        }

        private void AddAllLocalFarmersAsFriends(string[] args)
        {

        }

        private void AddFriend(string[] args)
        {
            if (args.Length == 3)
            {
                var name = args[0];
                var farmName = args[1];
                var id = args[2];
                // TODO: Replace
                //_farmerService.AddFriendToCurrentPlayer(name, farmName, id); 
                _mod.Monitor.Log($"{name} ({farmName} Farm) was added with id {id}.", LogLevel.Info);
            }
            else
            {
                LogArgumentsInvalid(_addFriendCommand);
            }
        }

        private void RemoveFriend(string[] args)
        {
            if (args.Length == 2 && args[0].ToLower() == "-id")
            {
                var id = args[1];
                var friend = _farmerService.CurrentFarmer.Friends.FirstOrDefault(x => x.Id == id);
                if (friend != null)
                {
                    // TODO: replace
                    //_farmerService.RemoveFriendFromCurrentPlayer(id); 
                    _mod.Monitor.Log($"{friend.Name} ({friend.FarmName} Farm) was removed!", LogLevel.Info);
                }
                else
                {
                    _mod.Monitor.Log($"Couldn'd find a friend with that id.", LogLevel.Info);
                }
            }
            else
            {
                LogArgumentsInvalid(_removeFriendCommand);
            }
        }

        private void RemoveAllFriend(string[] args)
        {

        }

        private void ListMyFriends(string[] args)
        {
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
