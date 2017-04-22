using System.Collections.Generic;
using Denifia.Stardew.SendItems.Domain;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IFarmerService
    {
        Farmer CurrentFarmer { get; }
        Farmer GetFarmerById(string id);
        List<Farmer> GetFarmers();
    }
}
