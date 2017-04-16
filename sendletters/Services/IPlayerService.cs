using denifia.stardew.sendletters.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public interface IPlayerService
    {
        void LoadCurrentPlayer();
        Player GetCurrentPlayer();
        Player GetPlayerById(string id);
    }
}
