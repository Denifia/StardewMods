//using System;
//using System.Collections.Generic;

//namespace Denifia.Stardew.SendItems.Domain
//{
//    public class Player
//    {
//        public string Name { get; set; }
//        public string FarmName { get; set; }
//        public List<Friend> Friends { get; set; }

//        private const int _randonStringLength = 5;

//        public Player()
//        {
//        }

//        public Player(string name, string farmName)
//        {
//            Name = name;
//            FarmName = farmName;
//            Id = string.Join(".", name, farmName, RandomString(_randonStringLength)).ToUpper();
//            Friends = new List<Friend>();
//            CreatedDate = DateTime.Now;
//        }
        
//        private string RandomString(int length)
//        {
//            string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
//            Random rng = new Random();
//            rng.Next();

//            char[] chars = new char[length];
//            int setLength = allowedChars.Length;

//            for (int i = 0; i < length; ++i)
//            {
//                chars[i] = allowedChars[rng.Next(setLength)];
//            }

//            return new string(chars, 0, length);
//        }
//    }
//}
