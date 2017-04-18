using Denifia.Stardew.SendLetters.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Services
{
    public interface IPlayerService
    {
        Player CurrentPlayer { get; set; }
        Player GetPlayerById(string id);
    }
}
