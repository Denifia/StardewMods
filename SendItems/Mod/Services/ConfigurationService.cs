﻿using Denifia.Stardew.SendLetters.Domain;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private IModHelper _modHelper;
        private ModConfig _modConfig;

        public ConfigurationService(IModHelper modHelper)
        {
            _modHelper = modHelper;
            _modConfig = _modHelper.ReadConfig<ModConfig>();
        }

        public Uri GetApiUri()
        {
            return _modConfig.ApiUrl;
        }

        public string GetLocalPath()
        {
            return _modHelper.DirectoryPath;
        }

        public bool InDebugMode()
        {
            return _modConfig.Debug;
        }

        public bool InLocalOnlyMode()
        {
            return !(_modConfig.ApiUrl != null && _modConfig.ApiUrl.OriginalString.Length > 0 && _modConfig.ApiUrl.IsAbsoluteUri);
        }

        public List<SavedGame> GetSavedGames()
        {
            var savedGames = new List<SavedGame>();
            string str = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"));
            if (Directory.Exists(str))
            {
                string[] directories = Directory.GetDirectories(str);
                if (directories.Length != 0)
                {
                    foreach (string path2 in directories)
                    {
                        try
                        {
                            FileInfo file = new FileInfo(Path.Combine(str, path2, "SaveGameInfo"));
                            if (file.Exists)
                            {
                                var fileContents = File.ReadAllText(file.FullName);

                                var farmerNodeStart = fileContents.IndexOf("<Farmer");
                                var farmerNodeEnd = fileContents.IndexOf("</Farmer>");
                                var farmerNode = fileContents.Substring(farmerNodeStart, farmerNodeEnd - farmerNodeStart);
                                var playerNameNodeStart = farmerNode.IndexOf("<name>") + 6;
                                var playerNameNodeEnd = farmerNode.IndexOf("</name>");
                                var playerName = farmerNode.Substring(playerNameNodeStart, playerNameNodeEnd - playerNameNodeStart);

                                var farmNameNodeStart = fileContents.IndexOf("<farmName>") + 10;
                                var farmNameNodeEnd = fileContents.IndexOf("</farmName>");
                                var farmName = fileContents.Substring(farmNameNodeStart, farmNameNodeEnd - farmNameNodeStart);

                                var savedGame = new SavedGame {
                                    Name = playerName,
                                    FarmName = farmName
                                };

                                savedGames.Add(savedGame);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            return savedGames;
        }
    }
}