using Denifia.Stardew.SendItems.Events;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using xTile.Dimensions;

namespace Denifia.Stardew.SendItems.Services
{
    public interface ILetterboxService
    {
        
    }

    public class LetterboxService
    {
        private const string locationOfLetterbox = "Farm";

        public LetterboxService()
        {
            SaveEvents.AfterLoad += AfterSavedGameLoad;
            ModEvents.PlayerCheckedLetterbox += PlayerCheckedLetterbox;
        }

        private void PlayerCheckedLetterbox(object sender, EventArgs e)
        {
            // TODO : Check letterbox
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
            LocationEvents.CurrentLocationChanged += CurrentLocationChanged;
            SaveEvents.AfterLoad -= AfterSavedGameLoad;
        }

        private void CurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            if (e.NewLocation.name == locationOfLetterbox)
            {
                // Only watch for mouse events while at the farm, for performance
                ControlEvents.MouseChanged += MouseChanged;
            }
            else
            {
                ControlEvents.MouseChanged -= MouseChanged;
            }
        }

        private void MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
            if (e.NewState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                // Check if the click is on the letterbox tile or the one above it
                Location tileLocation = new Location((int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y);

                if (tileLocation.X == 68 && (tileLocation.Y >= 15 && tileLocation.Y <= 16))
                {
                    ModEvents.RaisePlayerCheckedLetterbox(this, EventArgs.Empty);
                }
            }
        }
    }
}
