using System.Collections.Generic;
using Denifia.Stardew.SendItems.Domain;
using StardewValley;
using System.Linq;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IFarmerService
    {
        Domain.Farmer CurrentFarmer { get; }
        LoadCurrentFarmer();
        List<Domain.Farmer> GetFarmers();
        bool AddFriendToCurrentPlayer(Friend friend);
        bool RemoveFriendFromCurrentPlayer(string id);
        bool RemoveAllFriendFromCurrentPlayer();
    }

    public class FarmerService : IFarmerService
    {
        private readonly IConfigurationService _configService;

        private Domain.Farmer _currentFarmer;
        public Domain.Farmer CurrentFarmer {
            get { return _currentFarmer; }
        }

        public FarmerService(IConfigurationService configService)
        {
            _configService = configService;
        }

        public void LoadCurrentFarmer()
        {
            var newFarmer = new Domain.Farmer()
            {
                // TODO: Set the id to the save folder name
                Id = SaveGame.loaded.uniqueIDForThisGame.ToString(),
                Name = SaveGame.loaded.player.name,
                FarmName = SaveGame.loaded.player.farmName
            };

            var existingFarmer = GetFarmerById(newFarmer.Id);

            if (existingFarmer != null)
            {
                _currentFarmer = existingFarmer;
                return;
            }

            SaveFarmer(newFarmer);
            _currentFarmer = newFarmer;
        }

        public List<Domain.Farmer> GetFarmers()
        {
            return Repository.Instance.Fetch<Domain.Farmer>();
        }

        public bool AddFriendToCurrentPlayer(Friend friend)
        {
            if (CurrentFarmer == null) throw new System.Exception("current farmer is unknown");

            _currentFarmer.Friends.Add(friend);
            return Repository.Instance.Update(_currentFarmer);
        }

        public bool RemoveFriendFromCurrentPlayer(string id)
        {
            if (CurrentFarmer == null) throw new System.Exception("current farmer is unknown");

            var index = _currentFarmer.Friends.FindIndex(x => x.Id == id);
            if (index < 0) return false;

            _currentFarmer.Friends.RemoveAt(index);
            return Repository.Instance.Update(_currentFarmer);
        }

        public bool RemoveAllFriendFromCurrentPlayer()
        {
            _currentFarmer.Friends.Clear();
            return Repository.Instance.Update(_currentFarmer);
        }

        private void SaveFarmer(Domain.Farmer farmer)
        {
            Repository.Instance.Upsert(farmer);
        }

        private Domain.Farmer GetFarmerById(string id)
        {
            return Repository.Instance.SingleOrDefault<Domain.Farmer>(x => x.Id == id);
        }

        private void DetermineCurrentFarmer()
        {
            var name = Game1.player.name;
            var farmName = Game1.player.farmName;
            _currentFarmer = Repository.Instance.FirstOrDefault<Domain.Farmer>(x => x.Name == name && x.FarmName == farmName);
		}
    }
}
