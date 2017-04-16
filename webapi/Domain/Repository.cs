using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace denifia.stardew.webapi.Domain
{
    public class Repository
    {
        private static Repository instance;
        private readonly string _databaseFileName = "data.json";
        private FileInfo _database;

        public List<Message> Messages { get; set; }

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

        public void Init(string filePath)
        {
            _database = new FileInfo(Path.Combine(filePath, _databaseFileName));
            LoadDatabase();
        }

        public void LoadDatabase()
        {
            if (!_database.Exists)
            {
                File.WriteAllText(_database.FullName, "[]");
            }

            if (Messages == null)
            {
                Messages = JsonConvert.DeserializeObject<List<Message>>(File.ReadAllText(_database.FullName));
            }
        }

        public void SaveDatabase()
        {
            File.WriteAllText(_database.FullName, JsonConvert.SerializeObject(Messages));
        }
    }
}
