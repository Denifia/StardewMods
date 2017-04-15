using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace denifia.stardew.webapi.Domain
{
    public class Repository
    {
        private static Repository instance;
        private string FilePath;
        private readonly string DatabaseFile = "data.json";

        public List<Player> Players { get; set; }

        private Repository()
        {
        }

        public static Repository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Repository();
                }
                return instance;
            }
        }

        public void LoadDatabase(string path)
        {
            FilePath = path;
            if (Players == null)
            {
                Players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText(Path.Combine(FilePath, DatabaseFile)));
            }
        }

        public void SaveDatabase()
        {
            File.WriteAllText(Path.Combine(FilePath, DatabaseFile), JsonConvert.SerializeObject(Players));
        }
    }
}
