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
        Task<Domain.Farmer> GetFarmerByIdAsync(string id);
        Task<List<Domain.Farmer>> GetFarmersAsync();
        Task LoadCurrentFarmerAsync();
    }

    public class FarmerService : IFarmerService
    {
        private readonly IConfigurationService _configService;

        private Domain.Farmer _currentFarmer;
        public Domain.Farmer CurrentFarmer {
            get {
                if (_currentFarmer == null)
                {
                    Task.Run(DetermineCurrentFarmerAsync).Wait();
                }
                return _currentFarmer;
            }
        }

        public FarmerService(IConfigurationService configService)
        {
            _configService = configService;
        }

        public Task LoadCurrentFarmerAsync()
        {
            return Task.Run(() =>
            {
                var savedGames = _configService.GetSavedGames();
            });
        }

        public async Task<Domain.Farmer> GetFarmerByIdAsync(string id)
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    return db.Query<Domain.Farmer>().Where(x => x.Id == id).FirstOrDefault();
                }
            });
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
