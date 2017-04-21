using Denifia.Stardew.SendLetters.Domain;

namespace Denifia.Stardew.SendLetters.Services
{
    public interface IPlayerService
    {
        Player CurrentPlayer { get; }
        Player GetPlayerById(string id);
        void LoadLocalPlayers();
        void AddFriendToCurrentPlayer(string name, string farmName, string id);
        void RemoveFriendFromCurrentPlayer(string id);
    }
}
