using denifia.stardew.sendletters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public interface IMessageService
    {

        void CreateMessage(MessageCreateModel model);

    }
}
