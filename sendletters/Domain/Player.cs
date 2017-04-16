using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Domain
{
    public class Player
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string FarmName { get; set; }

        public List<Message> Messages { get; set; }

        public List<Player> Friends { get; set; }

        public Player(string name, string farmName)
        {
            Name = name;
            FarmName = farmName;
            Id = string.Join(".", name, farmName, RandomString(5)).ToUpper();

            Messages = new List<Message>();
            Friends = new List<Player>();
        }

        public Player()
        {

        }

        private static string RandomString(int length)
        {
            string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rng = new Random();

            char[] chars = new char[length];
            int setLength = allowedChars.Length;

            for (int i = 0; i < length; ++i)
            {
                chars[i] = allowedChars[rng.Next(setLength)];
            }

            return new string(chars, 0, length);
        }
    }
}
