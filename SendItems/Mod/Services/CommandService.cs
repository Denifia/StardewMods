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

    public class CommandService
    {
        private bool SavedGameLoaded = false;

        private readonly IMod _mod;
        private readonly IFarmerService _farmerService;

        public CommandService(
            IMod mod,
            IFarmerService farmerService)
        {
            _mod = mod;
            _farmerService = farmerService;

            SaveEvents.AfterLoad += AfterSavedGameLoad;
        }        

        public void RegisterCommands()
        {
            _mod.Helper.ConsoleCommands
                .Add("sendletters_friends", "Shows all the friends your currently loaded farmer.\n\nUsage: sendletters_friends -Name <name> -FarmName <farmName> -Id <id> \n- name: the name of your friend.\n- farmName: the name of your friends farm.\n- id: the id of your friend.", HandleCommand)
                .Add("sendletters_addfriend", "Adds a friend to your currently loaded farmer.\n\nUsage: sendletters_addfriend -Name <name> -FarmName <farmName> -Id <id> \n- name: the name of your friend.\n- farmName: the name of your friends farm.\n- id: the id of your friend.", HandleCommand)
                .Add("sendletters_removefriend", "Removes a friend from your currently loaded farmer.\n\nUsage: sendletters_removefriend -Id <id> \n- id: the id of your friend.", HandleCommand)
                .Add("sendletters_me", "Shows you the command that your friends need to type to add this current farmer as a friend.\n\nUsage: sendletters_me", HandleCommand);
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
