using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Denifia.Stardew.SendItems.Domain;
using LiteDB;
using StardewValley;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IFarmerService
    {
        Domain.Farmer CurrentFarmer { get; }
        Task LoadCurrentFarmerAsync();
        Task<List<Domain.Farmer>> GetFarmersAsync();
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

        public async Task LoadCurrentFarmerAsync()
        {
            var newFarmer = new Domain.Farmer()
            {
                Id = SaveGame.loaded.uniqueIDForThisGame.ToString(),
                Name = SaveGame.loaded.player.name,
                FarmName = SaveGame.loaded.player.farmName
            };

            var existingFarmer = await GetFarmerByIdAsync(newFarmer.Id);

            if (existingFarmer != null)
            {
                _currentFarmer = existingFarmer;
                return;
            }

            await SaveFarmerAsync(newFarmer);
            _currentFarmer = newFarmer;
        }

        public async Task<List<Domain.Farmer>> GetFarmersAsync()
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    return db.Query<Domain.Farmer>().ToList();
                }
            });
        }

        private async Task SaveFarmerAsync(Domain.Farmer farmer)
        {
            await Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    db.Insert(farmer);
                }
            });
        }

        private async Task<Domain.Farmer> GetFarmerByIdAsync(string id)
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    return db.Query<Domain.Farmer>().Where(x => x.Id == id).FirstOrDefault();
                }
            });
        }

        private Task DetermineCurrentFarmerAsync()
        {
            return Task.Run(() =>
			{
				var name = Game1.player.name;
				var farmName = Game1.player.farmName;
				using (var db = new LiteRepository(_configService.ConnectionString))
				{
					_currentFarmer = db.Query<Domain.Farmer>().Where(x => x.Name == name && x.FarmName == farmName).FirstOrDefault();
				}
			});
		}
    }
}
