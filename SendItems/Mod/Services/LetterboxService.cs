using Denifia.Stardew.SendItems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendItems.Services
{
    public interface ILetterboxService
    {

    }

    public class LetterboxService
    {
        public LetterboxService()
        {
            ModEvents.PlayerCheckedLetterbox += PlayerCheckedLetterbox;
        }

        private void PlayerCheckedLetterbox(object sender, EventArgs e)
        {
            // TODO: Complete PlayerCheckedLetterbox method
        }
    }
}
