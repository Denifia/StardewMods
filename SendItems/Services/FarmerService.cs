using System.Collections.Generic;
using Denifia.Stardew.SendItems.Domain;
using StardewValley;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IFarmerService
    {
        Domain.Farmer CurrentFarmer { get; }
        LoadCurrentFarmer();
        List<Domain.Farmer> GetFarmers();
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
