namespace Denifia.Stardew.SendLetters.Models
{
    public class CreateMessageModel
    {
        public string FromPlayerId { get; set; }
        public string ToPlayerId { get; set; }
        public string Text { get; set; }
    }
}
